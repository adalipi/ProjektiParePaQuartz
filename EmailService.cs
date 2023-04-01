using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace ProjektiPare
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _emailSettings = new EmailSettings();
            _configuration.GetSection("EmailSettings").Bind(_emailSettings);
        }

        public async Task SendEmailAsync(EmailData emailData)
        {
            SmtpClient emailClient = new (_emailSettings.Host, _emailSettings.Port);
            try
            {
                emailClient.EnableSsl = _emailSettings.UseSSL;
                
                if (_emailSettings.UseAuthentication)
                    emailClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);

                _logger.LogInformation($"Preparing to send email to: {emailData.ToEmail}, with subject {emailData.Subject}");

                var fromEmail = new MailAddress(_emailSettings.Email, _emailSettings.Name);
                var toEmail = new MailAddress(emailData.ToEmail, emailData.ToName);

                var emailMessage = new MailMessage(fromEmail, toEmail);

                if (emailData.CCs != null)
                {
                    foreach (var cc in emailData.CCs)
                    {
                        var adresa = new MailAddress(cc.Key, cc.Value);
                        emailMessage.CC.Add(adresa);
                    }
                }

                emailMessage.Subject = emailData.Subject;

                emailMessage.Body = emailData.Body;

                emailMessage.IsBodyHtml = true; // default false


                if (emailData.Attachments != null)
                {
                    foreach (var item in emailData.Attachments)
                    {
                        emailMessage.Attachments.Add(new Attachment(item));
                    }
                }


                emailMessage.ReplyToList.Add(new MailAddress("a.dalipi@live.com"));

                await emailClient.SendMailAsync(emailMessage);
                _logger.LogInformation($"Finished sending: {emailData.ToEmail}, with subject {emailData.Subject}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to: {emailData.ToEmail}, with subject {emailData.Subject}.");
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                emailClient.Dispose();
            }
        }
    }
}
