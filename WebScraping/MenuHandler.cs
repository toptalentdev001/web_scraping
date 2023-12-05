using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping
{
    internal class MenuHandler
    {
        private Menu menu = new();
        
        public MenuHandler(Menu menu) {
            this.menu = menu;
        }

        public void ProcessUserSelection()
        {
            string userSelection = menu.GetUserSelection();

            switch (userSelection)
            {
                case "Youtube": YoutubeScraper.ScrapeVideos();
                    break;
                case "Ictjobs.be": Console.WriteLine("Ictjobs.be");
                    break;
                case "Azerty.nl": Console.WriteLine("Azerty.nl");
                    break;
            }
        }
    }
}
