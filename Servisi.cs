using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektiPare
{
    public class Servisi
    {
        private readonly ILogger<Servisi> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IProgramMenaxheri _manageri;

        public Servisi(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            _logger = _serviceProvider.GetRequiredService<ILogger<Servisi>>();
            _manageri = _serviceProvider.GetRequiredService<IProgramMenaxheri>();
        }

        public void Fillo()
        {
            _logger.LogInformation("Servisi startoi");
            Planifikuesi();
        }

        public void Ndalo() 
        {
            _logger.LogInformation("Servisi ndaloi");
        }

        void Planifikuesi()
        {
            if (!int.TryParse(_configuration["FrekuencaVezhgimit"], out var frekuenca))
                frekuenca = 2;

            var timer = new Timer(async _ => await _manageri.BejPunen());
            timer.Change(0, frekuenca * 1000);  //shumezim me 1000 milisekonda qe te kthehet vlera ne sekonda 
        }
    }
}
