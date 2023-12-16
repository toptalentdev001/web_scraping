using OpenQA.Selenium;
using static QRCoder.PayloadGenerator;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace WebScraping
{
    public class YoutubeScraper
    {
        public static List<Video> videoList = new List<Video>();
        public static List<string> videoDetails = new List<string>
        {
            "Title",
            "Author",
            "ViewCount",
            "UploadTimestamp",
            "Url",
            "Thumbnail"
        };

        public YoutubeScraper() { }

        private static string GetSearchTerm()
        {
            string searchTerm;

            do
            {
                Console.Write("\n[?] Enter a search term (Youtube): ");
                searchTerm = Console.ReadLine();

            } while (string.IsNullOrEmpty(searchTerm) && string.IsNullOrWhiteSpace(searchTerm));

            Console.WriteLine($"\n[i] Searching for \"{searchTerm}\" on Youtube ...\n");

            // Encode special characters
            searchTerm = Uri.EscapeDataString(searchTerm);

            return searchTerm;
        }

        public static List<string> ExtractElements(IReadOnlyList<IWebElement> elements, int count)
        {
            List<string> extractedTexts = new List<string>();

            for (int i = 0; extractedTexts.Count < 5; i++)
            {
                if (elements[i].Text.Length > 0)
                {
                    extractedTexts.Add(elements[i].Text);
                }
            }

            return extractedTexts;
        }

        public static List<string> GetVideoTitles(IWebDriver driver)
        {
            var videos = driver.FindElements(By.Id("video-title"));
            List<string> videoTitles = ExtractElements(videos, 5);

            return videoTitles;
        }

        public static List<string> GetVideoAuthors(IWebDriver driver)
        {
            var channels = driver.FindElements(By.CssSelector(".yt-simple-endpoint.style-scope.yt-formatted-string"));
            List<string> authors = ExtractElements(channels, 5);

            return authors;
        }

        public static List<string> GetVideoUrls(IWebDriver driver)
        {
            var urls = driver.FindElements(By.CssSelector(".yt-simple-endpoint.inline-block.style-scope.ytd-thumbnail"));
            List<string> videoUrls = new List<string>();

            for (int i = 0; videoUrls.Count < 5; i++)
            {
                var videoUrl = urls[i].GetAttribute("href");

                if (videoUrl != null)
                {
                    // Define the regular expression pattern
                    string pattern = @"&pp=[^&]+";

                    // Use Regex.Replace to remove the matched substring
                    string videoLink = Regex.Replace(videoUrl, pattern, "");

                    videoUrls.Add(videoLink);
                }
            }

            return videoUrls;
        }

        public static List<string> GetVideoViews(IWebDriver driver)
        {
            var views = driver.FindElements(By.CssSelector(".inline-metadata-item.style-scope.ytd-video-meta-block"));
            List<string> videoViews = new List<string>();

            for (int i = 0; videoViews.Count < 5; i++)
            {
                if (i % 2 == 0) // Check if i is even
                {
                    var videoViewCount = views[i].Text;

                    if (videoViewCount.Length >= 0)
                    {
                        videoViews.Add(videoViewCount);
                    }
                }
            }

            return videoViews;
        }

        public static List<string> GetVideoUploadTimes(IWebDriver driver)
        {
            var views = driver.FindElements(By.CssSelector(".inline-metadata-item.style-scope.ytd-video-meta-block"));
            List<string> videoUploadTimestamps = new List<string>();

            for (int i = 0; videoUploadTimestamps.Count < 5; i++)
            {
                if (i % 2 == 1) // Check if i is odd
                {
                    var videoViewCount = views[i].Text;

                    if (videoViewCount.Length >= 0)
                    {
                        videoUploadTimestamps.Add(videoViewCount);
                    }
                }
            }

            return videoUploadTimestamps;
        }

        public static void AddVideosToList(Video video)
        {
            videoList.Add(video);
        }

        public static void ScrapeVideos()
        {
            IWebDriver driver = WebDriverFactory.InitializeChromeDriver();
            string youtubeSearchTerm = GetSearchTerm();

            // Most popular search results
            driver.Navigate().GoToUrl($"https:www.youtube.com/results?search_query={youtubeSearchTerm}");

            // Most recent search results
            //driver.Navigate().GoToUrl($"https:www.youtube.com/results?search_query={youtubeSearchTerm}&sp=CAI%253D");

            try
            {
                // Find the reels element to remove
                IWebElement reelsDiv = driver.FindElement(By.CssSelector("ytd-reel-shelf-renderer.style-scope.ytd-item-section-renderer"));

                // Execute JavaScript to remove the div element from the DOM
                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                jsExecutor.ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", reelsDiv);
            } catch (OpenQA.Selenium.NoSuchElementException ex) {}

            var videoTitles = GetVideoTitles(driver);
            var videoViews = GetVideoViews(driver);
            var videoAuthors = GetVideoAuthors(driver);
            var videoUploadTimes = GetVideoUploadTimes(driver);
            var videoUrls = GetVideoUrls(driver);
            List<string> videoThumbnails = new();
            
            // Add thumbnails
            for (int i = 0; i < 5; i++)
            {
                var url = videoUrls[i];
                string pattern = @"(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})";
                
                // Use Regex.Match to find the first match
                Match match = Regex.Match(url, pattern);

                if (match.Success)
                {
                    // Extract the video ID from the match
                    string videoId = match.Groups[1].Value;

                    // Display the extracted video ID
                    string videoThumbnailUrl = $"https://i.ytimg.com/vi/{videoId}/hq720.jpg";
                    videoThumbnails.Add(videoThumbnailUrl);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"*---------------------------------------------*");
                Console.WriteLine($"| Titel: {videoTitles[i]} - {videoAuthors[i]}");
                Console.WriteLine($"| Views: {videoViews[i]}");
                Console.WriteLine($"| Geüpload: {videoUploadTimes[i]}");
                Console.WriteLine($"| URL: {videoUrls[i]}");
                Console.WriteLine($"| Thumbnail: {videoThumbnails[i]}");
                Console.WriteLine($"*---------------------------------------------*");

                // Create Video objects and add to list
                Video CurrentVideo = new Video();
                CurrentVideo.Title = videoTitles[i];
                CurrentVideo.Author = videoAuthors[i];
                CurrentVideo.ViewCount = videoViews[i];
                CurrentVideo.UploadTimestamp = videoUploadTimes[i];
                CurrentVideo.Url = videoUrls[i];
                CurrentVideo.Thumbnail = videoThumbnails[i];

                AddVideosToList(CurrentVideo);
            }

            // Save data to CSV file
            ExportCsv.CreateCsvFile("videos", videoList, videoDetails);
            ExportJson.CreateJsonFile("videos", videoList, videoDetails);

            // Quit driver
            WebDriverFactory.QuitDriver(driver);
        }
    }
}
