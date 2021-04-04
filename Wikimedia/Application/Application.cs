using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wikimedia.Domain;
using Wikimedia.Infraestructure;

namespace Wikimedia.Application
{
    class Application
    {
        static void Main(string[] args)
        {
            List<Date> dates = new List<Date>();
            List<Page> pages = new List<Page>();
            PageDomain wikimedia = new PageDomain();
            dates = wikimedia.ObtainDates();
            wikimedia.GetFiles(dates);
            wikimedia.DecompressAll();
            wikimedia.ObtainAllMaxViewPerPage(dates);
            wikimedia.ObtainAllMaxViewLD(dates);
        }
    }
}
