using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping
{
    public class Job
    {
        private string title;
        private string company;
        private List<string> keywords;
        private string detailsUrl;
        
        public Job() { }

        public string Title { get => title; set => title = value; }
        public string Company { get => company; set => company = value; }
        public List<string> Keywords { get => keywords; set => keywords = value; }
        public string DetailsUrl { get => detailsUrl; set => detailsUrl = value; }
    }
}
