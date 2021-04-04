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
            List<DateEntity> dates = new List<DateEntity>();
            List<PageView> pages = new List<PageView>();
            PageViewDomain wikimedia = new PageViewDomain();
            dates = wikimedia.ObtainDates();
            wikimedia.GetFiles(dates);
            wikimedia.DecompressAll();
            wikimedia.ObtainAllMaxViewPerPage(dates);
            wikimedia.ObtainAllMaxViewLD(dates);
        }
    }
}
