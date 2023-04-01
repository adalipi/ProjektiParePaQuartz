
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using ProjektiPare;
using Topshelf;

IConfiguration configFile = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();

var services = new ServiceCollection();
services
    .AddLogging(log => { log.ClearProviders(); log.AddNLog(); })
    .AddSingleton(configFile)
    .AddScoped<IFajllManipuluesi, FajllManipuluesi>()
    .AddScoped<IProgramMenaxheri, ProgramMenaxheri>()
    .AddScoped<IEmailService, EmailService>()
    ;

using (var serviceProvider = services.BuildServiceProvider())
{
    HostFactory.Run(ser => 
    {
        ser.SetServiceName("ProjektiPare");
        ser.SetDisplayName("ProjektiPare");
        ser.SetDescription("Projekti i pare ne dot net shqip");
       
        ser.Service<Servisi>(s => 
        {
            s.ConstructUsing(_ => new Servisi(serviceProvider));
            s.WhenStarted(ss => ss.Fillo());
            s.WhenStopped(ss => ss.Ndalo());
        });
    });
}

