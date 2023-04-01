using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektiPare
{
    public class FajllManipuluesi : IFajllManipuluesi
    {
        private readonly string _filePath;

        public FajllManipuluesi(IConfiguration configuration)
        {
            _filePath = configuration["Folderi"];
        }

        public void Fshijfajllat()
        {
            foreach (var item in Directory.EnumerateFiles(_filePath))
            {
                File.Delete(item);
            }
        }

        public IEnumerable<string> LexoPermbajtjen()
        {
            foreach (var item in Directory.EnumerateFiles(_filePath))
            {
                yield return File.ReadAllText(item);
            }
        }
    }
}
