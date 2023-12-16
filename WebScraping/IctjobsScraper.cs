using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace WebScraping
{
    public class IctjobsScraper
    {
        public static List<Job> jobsList = new List<Job>();
        public static List<string> jobDetails = new List<string>
        {
            "Title",
            "Company",
            "Location",
            "DatePosted",
            "DetailsUrl",
            "OrganizationImage"
        };

        public IctjobsScraper() {}

        private static string GetSearchTerm()
        {
            string searchTerm;

            do
            {
                Console.Write("\n[?] Enter a search term (Ictjobs.be): ");
                searchTerm = Console.ReadLine();

            } while (string.IsNullOrEmpty(searchTerm) || string.IsNullOrWhiteSpace(searchTerm));

            Console.WriteLine($"\nSearching for \"{searchTerm}\" on Ictjob.be ...\n");
            
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

        public static List<string> GetJobTitles(IWebDriver driver)
        {
            var jobs = driver.FindElements(By.ClassName("job-title"));
            var jobTitles = ExtractElements(jobs, 5);

            return jobTitles;
        }

        public static List<string> GetJobCompanies(IWebDriver driver)
        {
            var companies = driver.FindElements(By.ClassName("job-company"));
            var jobCompanies = ExtractElements(companies, 5);

            return jobCompanies;
        }

        public static List<string> GetJobLocations(IWebDriver driver)
        {
            var locations = driver.FindElements(By.XPath("//span[contains(@itemprop, 'addressLocality')]"));
            var jobLocations = ExtractElements(locations, 5);

            return jobLocations;
        }

        public static List<string> GetJobDates(IWebDriver driver)
        {
            var dates = driver.FindElements(By.XPath("//span[contains(@itemprop, 'datePosted')]"));
            var jobDates = ExtractElements(dates, 5);

            return jobDates;
        }

        public static List<string> GetJobUrls(IWebDriver driver)
        {
            var urls = driver.FindElements(By.CssSelector(".job-title.search-item-link"));
            List<string> jobUrls = new List<string>();

            for (int i = 0; jobUrls.Count < 5; i++)
            {
                var jobUrl = urls[i].GetAttribute("href");

                if (jobUrl != null)
                {
                    jobUrls.Add(jobUrl);
                }
            }

            return jobUrls;
        }

        public static List<string> GetOrganizationImages(IWebDriver driver)
        {
            var images = driver.FindElements(By.CssSelector(".search-item-logo.company-logo-small"));
            List<string> organizationImages = new List<string>();

            for (int i = 0; organizationImages.Count < 5; i++)
            {
                var smallImageUrl = images[i].GetAttribute("src");

                if (smallImageUrl != null)
                {
                    string imageUrl = Regex.Replace(smallImageUrl, @"\.small\.png$", "");
                    organizationImages.Add(imageUrl);
                }
            }

            return organizationImages;
        }

        public static void AddJobsToList(Job job)
        {
            jobsList.Add(job);
        }

        public static void ScrapeJobs()
        {
            IWebDriver driver = WebDriverFactory.InitializeChromeDriver();
            string jobSearchTerm = GetSearchTerm();
            driver.Navigate().GoToUrl($"https://www.ictjob.be/en/search-it-jobs?keywords={jobSearchTerm}");
            driver.Navigate().Refresh();

            IWebElement cookieButton = driver.FindElement(By.CssSelector(".button.cookie-layer-button.close-layer-button"));
            IWebElement sortByDateButton = driver.FindElement(By.XPath("//*[@id=\"sort-by-date\"]"));

            cookieButton.Click();
            sortByDateButton.Click();
            Thread.Sleep(TimeSpan.FromSeconds(10));

            var vacancies = GetJobTitles(driver);
            var hiringOrganizations = GetJobCompanies(driver);
            var vacancyLocations = GetJobLocations(driver);
            var vacancyUrls = GetJobUrls(driver);
            var datesPosted = GetJobDates(driver);
            var OrganizationImages = GetOrganizationImages(driver);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"*---------------------------------------------*");
                Console.WriteLine($"| {vacancies[i]}");
                Console.WriteLine($"| @ {hiringOrganizations[i]}");
                Console.WriteLine($"| in {vacancyLocations[i]}");
                Console.WriteLine($"| posted on {datesPosted[i]}");
                Console.WriteLine($"| URL: {vacancyUrls[i]}");
                Console.WriteLine($"| Image: {OrganizationImages[i]}");
                Console.WriteLine($"*---------------------------------------------*\n");

                // Create Job objects
                Job jobOpportunity = new Job();
                jobOpportunity.Title = vacancies[i];
                jobOpportunity.Company = hiringOrganizations[i];
                jobOpportunity.Location = vacancyLocations[i];
                jobOpportunity.DatePosted = datesPosted[i];
                jobOpportunity.DetailsUrl = vacancyUrls[i];
                jobOpportunity.OrganizationImage = OrganizationImages[i];

                // Add Jobs to list of jobs
                AddJobsToList(jobOpportunity);
            }

            // Create CSV 
            ExportCsv.CreateCsvFile("jobs", jobsList, jobDetails);
            ExportJson.CreateJsonFile("jobs", jobsList, jobDetails);

            // Quit driver
            WebDriverFactory.QuitDriver(driver);
        }
    }
}
