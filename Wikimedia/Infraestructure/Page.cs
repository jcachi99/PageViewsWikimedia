using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Wikimedia.Infraestructure
{
    class Page
    {
        public String period { get; set; }
        public String language { get; set; }
        public String domain { get; set;}
        public String pageTitle { get; set; }
        public int viewCount { get; set; }

        public Page(string period, string language, string domain, string pageTitle, int viewCount)
        {
            this.period = period;
            this.language = language;
            this.domain = domain;
            this.pageTitle = pageTitle;
            this.viewCount = viewCount;
        }
    }
}
