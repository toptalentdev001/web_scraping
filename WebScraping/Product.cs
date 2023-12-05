using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping
{
    public class Product {
        private string name;
        private string description;
        private string deliveryTime;
        private decimal price;
        private string url;
        private string imageUrl;

        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string DeliveryTime { get => deliveryTime; set => deliveryTime = value; }
        public decimal Price { get => price; set => price = value; }
        public string Url { get => url; set => url = value; }
        public string ImageUrl { get => imageUrl; set => imageUrl = value; }
    }
}
