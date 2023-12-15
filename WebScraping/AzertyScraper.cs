using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebScraping;
public class AzertyScraper
{
    public static  List<Product> productList = new List<Product>();
    public static List<string> productDetails = new List<string>
    {
        "Name",
        "Price",
        "Description",
        "DeliveryTime",
        "Url",
        "ImageUrl"
    };

    AzertyScraper() { }     
    private static string GetSearchTerm()
    {
        string searchTerm;

        do
        {
            Console.Write("\n[?] Enter a search term (Azerty.nl): ");
            searchTerm = Console.ReadLine();

        } while (string.IsNullOrEmpty(searchTerm) || string.IsNullOrWhiteSpace(searchTerm));

        Console.WriteLine($"\nSearching for \"{searchTerm}\" on Azerty.nl ...\n");

        // Encode special characters
        searchTerm = Uri.EscapeDataString(searchTerm);

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

        decimal productPrice = decimal.Parse(price.Text, CultureInfo.CurrentCulture);

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

        string pattern = @"(.*\.(jpg|jpeg|png))";
        string imageUrl = image.GetAttribute("src");

        Match match = Regex.Match(imageUrl, pattern);

        if (match.Success)
        {
            imageUrl = match.Groups[1].Value;
        }

        return imageUrl;
    }

    public static void CreateProducts(IWebDriver driver)
    {
        var products = driver.FindElements(By.CssSelector(".item.product.product-item.product_addtocart_form.card"));

        for (int i = 0; i < 5; i++)
        {
            IWebElement productFound = products[i];

            // Create Product objects from search results
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

            Console.WriteLine($"*---------------------------------------------*\n" +
                $"| Product: {FoundProduct.Name} - €{FoundProduct.Price}\n" +
                $"| Description: {FoundProduct.Description}\n" +
                $"| Delivery time: {FoundProduct.DeliveryTime}\n" +
                $"| URL: {FoundProduct.Url}\n" +
                $"| Product image: {FoundProduct.ImageUrl}\n" +
                $"*---------------------------------------------*");
        }
    }

    private static void RefuseCookies(IWebDriver driver)
    {
        // Wait for banner animation (ease-in)
        Thread.Sleep(3000);
            
        // Find button and click on reject
        var cookieRefuseButton = driver.FindElement(By.Id("CybotCookiebotDialogBodyButtonDecline"));
        cookieRefuseButton.Click();
            
        // Wait for banner animation (ease-out)
        Thread.Sleep(3000);
    }

    public static void ScrapeProducts()
    {
        IWebDriver driver = WebDriverFactory.InitializeChromeDriver();

        // Go to website
        string searchTerm = GetSearchTerm();
        string scrapingUrl = $"https://azerty.nl/catalogsearch/result/?q={searchTerm}";
        driver.Navigate().GoToUrl(scrapingUrl);
            
        // Refuse cookie banner to proceed with scraping
        RefuseCookies(driver);

        // Call methods to create Product objects
        CreateProducts(driver);
            
        // Console.WriteLine Products
        showProducts();

        // Create CSV file from products
        ExportCsv.CreateCsvFile("products", productList, productDetails);
        ExportJson.CreateJsonFile("products", productList, productDetails);

        // Quit driver
        WebDriverFactory.QuitDriver(driver);
    }
}
