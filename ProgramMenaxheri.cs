using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektiPare
{
    public class ProgramMenaxheri : IProgramMenaxheri
    {
        private readonly ILogger<ProgramMenaxheri> _logger;
        private readonly IEmailService _emailService;
        private readonly IFajllManipuluesi _fileManipuluesi;

        public ProgramMenaxheri(ILogger<ProgramMenaxheri> logger, IEmailService emailService, IFajllManipuluesi fileManipuluesi)
        {
            _logger = logger;
            _emailService = emailService;
            _fileManipuluesi = fileManipuluesi;
        }

        public async Task BejPunen()
        {
            try
            {
                foreach (var item in _fileManipuluesi.LexoPermbajtjen())
                {
                    await _emailService.SendEmailAsync(new EmailData { Body = item, Subject = "test", ToEmail = "a.dalipi@live.com", ToName = "Arjan" });
                }

                _fileManipuluesi.Fshijfajllat();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Ndodhi gabim...");
                throw;
            }
        }
    }
}
