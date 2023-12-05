using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping
{
    public class AzertyScraper
    {
        public static IWebDriver driver = WebDriverFactory.InitializeChromeDriver();
        public static List<Product> productList = new List<Product>();
        
        AzertyScraper() { }
        private static string GetSearchTerm()
        {
            string searchTerm;

            do
            {
                Console.Write("\nEnter a search term: ");
                searchTerm = Console.ReadLine();

            } while (string.IsNullOrEmpty(searchTerm) || string.IsNullOrWhiteSpace(searchTerm));

            Console.WriteLine($"\nSearching for {searchTerm} on Azerty.nl ...\n");

            return searchTerm;
        }

        public static string GetProductName(IWebElement cardElement)
        {
            IWebElement productName = cardElement.FindElement(By.CssSelector(".product-item-link"));

            return productName.Text;
        }

        public static string GetProductDescription(IWebElement cardElement)
        {
            IWebElement productDescription = cardElement.FindElement(By.CssSelector(".product-item-description"));

            return productDescription.Text;
        }

        public static decimal GetProductPrice(IWebElement cardElement)
        {
            var price = cardElement.FindElement(By.CssSelector(".price"));

            decimal productPrice = decimal.Parse(price.Text, CultureInfo.InvariantCulture);

            return productPrice;
        }

        public static string GetDeliveryTime(IWebElement cardElement)
        {
            IWebElement deliveryTime = cardElement.FindElement(By.CssSelector(".product-delivery-time"));

            string productDeliveryTime = deliveryTime.Text;

            return productDeliveryTime;
        }

        public static string GetProductUrl(IWebElement cardElement)
        {
            IWebElement url = cardElement.FindElement(By.CssSelector(".product-item-link"));

            string productUrl = url.GetAttribute("href");

            return productUrl;
        }

        public static string GetProductImage(IWebElement cardElement)
        {
            IWebElement image = cardElement.FindElement(By.CssSelector(".object-contain.product-image-photo"));

            string imageUrl = image.GetAttribute("src");

            return imageUrl;
        }

        public static void CreateProducts()
        {
            var products = driver.FindElements(By.CssSelector(".item.product.product-item.product_addtocart_form.card"));
            Console.WriteLine($"Products {products.Count} products");
            for (int i = 0; i < 5; i++)
            {
                IWebElement productFound = products[i];
                Product ShoppingProduct= new Product();

                ShoppingProduct.Name = GetProductName(productFound);
                ShoppingProduct.Price = GetProductPrice(productFound);
                ShoppingProduct.Description = GetProductDescription(productFound);
                ShoppingProduct.DeliveryTime = GetDeliveryTime(productFound);
                ShoppingProduct.Url = GetProductUrl(productFound);
                ShoppingProduct.ImageUrl = GetProductImage(productFound);
                
                // Add product to lists
                productList.Add(ShoppingProduct);
            }
        }

        public static void showProducts()
        {
            for (int i = 0; i < productList.Count; i++)
            {
                Product FoundProduct = productList[i];

                //Console.WriteLine($"*---------------------------------------------*");
                //Console.WriteLine($"| Product: {FoundProduct.Name} - {FoundProduct.Price}");
                //Console.WriteLine($"| Description: {FoundProduct.Description} ");
                //Console.WriteLine($"| Delivery time: {FoundProduct.DeliveryTime}");
                //Console.WriteLine($"| URL: {FoundProduct.Url}");
                //Console.WriteLine($"| Product image: {FoundProduct.ImageUrl}");
                //Console.WriteLine($"*---------------------------------------------*\n");

                Console.WriteLine($"*---------------------------------------------*\n" +
                  $"| Product: {FoundProduct.Name} - {FoundProduct.Price}\n" +
                  $"| Description: {FoundProduct.Description}\n" +
                  $"| Delivery time: {FoundProduct.DeliveryTime}\n" +
                  $"| URL: {FoundProduct.Url}\n" +
                  $"| Product image: {FoundProduct.ImageUrl}\n" +
                  $"*---------------------------------------------*");

            }
        }

        private static void RefuseCookies()
        {
            Thread.Sleep(3000);
            var cookieRefuseButton = driver.FindElement(By.Id("CybotCookiebotDialogBodyButtonDecline"));
            cookieRefuseButton.Click();
            Thread.Sleep(3000);
        }

        public static void ScrapeProducts()
        {
            string searchTerm = GetSearchTerm();
            string scrapingUrl = $"https://azerty.nl/catalogsearch/result/?q={searchTerm}";
            driver.Navigate().GoToUrl(scrapingUrl);
            
            RefuseCookies();

            CreateProducts();
            showProducts();

            driver.Quit();
        }
    }
}
