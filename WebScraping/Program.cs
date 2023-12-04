using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Selenium;

public static class HelloSelenium
{
    public static void Main()
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments("headless", "--silent", "log-level=3");
        
        var chromeDriverService = ChromeDriverService.CreateDefaultService();
        chromeDriverService.HideCommandPromptWindow = true;
        chromeDriverService.SuppressInitialDiagnosticInformation = true;
        chromeDriverService.EnableVerboseLogging = false;


        IWebDriver driver = new ChromeDriver(chromeDriverService, chromeOptions);

        Console.WriteLine("Welcome to Youtube Web Scraper!\n");

        Console.Write("Enter search term: ");
        string searchTerm = Console.ReadLine();
        driver.Navigate().GoToUrl($"https://www.youtube.com/results?search_query={searchTerm}");

        // driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        var videos = driver.FindElements(By.Id("video-title"));
        var channels = driver.FindElements(By.CssSelector(".yt-simple-endpoint.style-scope.yt-formatted-string"));
        var views = driver.FindElements(By.CssSelector(".inline-metadata-item.style-scope.ytd-video-meta-block"));
        // var urls = driver.FindElements(By.CssSelector("yt-simple-endpoint inline-block style-scope ytd-thumbnail"));
        var videoTitlesList = new List<string>();

        // filter empty channel entries
        for (int i = 0; i < channels.Count; i++)
        {
            var videoTitle = channels[i];

            if (videoTitle.Text.Length > 0)
            {
                videoTitlesList.Add(videoTitle.Text);
            }
        }

        Console.WriteLine("---------------------------------");
        Console.WriteLine($"\nTotal search results: {videos.Count}\n");
        Console.WriteLine("First 5 search results:\n");

        for (int i = 0; i < 5; i++)
        {
            var videoTitles = videos[i].Text;
            var uploadedBy = videoTitlesList[i];
            var videoViews = views[i].Text;
            var index = 1 + i;

            Console.WriteLine($"{index}- {videoTitles} | Uploaded by: {uploadedBy} | Views: {videoViews}");
        }

        driver.Quit();
    }
}