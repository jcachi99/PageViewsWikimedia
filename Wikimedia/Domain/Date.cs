using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wikimedia.Domain
{
    class Date
    {

        public String year { get; set; }
        public String month { get; set; }
        public String day { get; set; }
        public String hour { get; set; }

        public Date(string year, string month, string day, string hour)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
        }
    }
}
