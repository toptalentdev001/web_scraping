﻿using OpenQA.Selenium;

namespace WebScraping
{
    public class YoutubeScraper
    {
        public static IWebDriver driver = WebDriverFactory.InitializeChromeDriver();
        public static List<Video> videoList = new List<Video>();

        public YoutubeScraper() { }

        private static string GetSearchTerm()
        {
            string searchTerm;

            do
            {
                Console.Write("\nEnter a search term: ");
                searchTerm = Console.ReadLine();

            } while (string.IsNullOrEmpty(searchTerm) && string.IsNullOrWhiteSpace(searchTerm));

            Console.WriteLine($"\nSearching for {searchTerm} on Youtube ...\n");

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

        public static List<string> GetVideoTitles()
        {
            var videos = driver.FindElements(By.Id("video-title"));
            List<string> videoTitles = ExtractElements(videos, 5);

            return videoTitles;
        }

        public static List<string> GetVideoAuthors()
        {
            var channels = driver.FindElements(By.CssSelector(".yt-simple-endpoint.style-scope.yt-formatted-string"));
            List<string> authors = ExtractElements(channels, 5);

            return authors;
        }

        public static List<string> GetVideoUrls()
        {
            var urls = driver.FindElements(By.CssSelector(".yt-simple-endpoint.inline-block.style-scope.ytd-thumbnail"));
            List<string> videoUrls = new List<string>();

            for (int i = 0; videoUrls.Count < 5; i++)
            {
                var videoUrl = urls[i].GetAttribute("href");

                if (videoUrl != null)
                {
                    videoUrls.Add(videoUrl);
                }
            }

            return videoUrls;
        }

        public static List<string> GetVideoViews()
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

        public static List<string> GetVideoUploadTimes()
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

        public static void ScrapeVideos()
        {
            string youtubeSearchTerm = GetSearchTerm();
            //driver.Navigate().GoToUrl($"https:www.youtube.com/results?search_query={youtubeSearchTerm}");

            // Most recent search results
            driver.Navigate().GoToUrl($"https:www.youtube.com/results?search_query={youtubeSearchTerm}&sp=CAI%253D");

            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            var videoTitles = GetVideoTitles();
            var videoViews = GetVideoViews();
            var videoAuthors = GetVideoAuthors();
            var videoUploadTimes = GetVideoUploadTimes();
            var videoUrls = GetVideoUrls();

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"*---------------------------------------------*");
                Console.WriteLine($"| {videoTitles[i]}");
                Console.WriteLine($"| Uploaded by: {videoAuthors[i]}");
                Console.WriteLine($"| Views: {videoViews[i]}");
                Console.WriteLine($"| {videoUploadTimes[i]}");
                Console.WriteLine($"| URL: {videoUrls[i]}");
                Console.WriteLine($"*---------------------------------------------*\n");

                // Create Video objects and add to list
                Video CurrentVideo = new Video();
                CurrentVideo.Title = videoTitles[i];
                CurrentVideo.Author = videoAuthors[i];
                CurrentVideo.ViewCount = videoViews[i];
                CurrentVideo.UploadTimestamp = videoUploadTimes[i];
                CurrentVideo.Url = videoUrls[i];

                AddVideosToList(CurrentVideo);
            }

            driver.Quit();

            // Ask for menu
            MenuHandler menuHandler = new();
            menuHandler.AskForMenu();
        }

        public static void AddVideosToList(Video video)
        {
            videoList.Add(video);
        }
    }
}
