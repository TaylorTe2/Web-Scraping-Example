using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web_Scraping_Example.Models;
using HtmlAgilityPack;
using PuppeteerSharp;
using System.Reflection.Metadata;

namespace Web_Scraping_Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            string url = "https://en.wikipedia.org/wiki/List_of_programmers";

            List<string> programmerLinks = new List<string>();

            var options = new LaunchOptions()
            {
                Headless = true,
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"

            };

            var browser = await Puppeteer.LaunchAsync(options, null);
            var page = await browser.NewPageAsync();

            await page.GoToAsync(url);

            var links = @"Array.from(document.querySelectorAll('li:not([class])')).map(li => {
        const firstAnchor = li.querySelector('a');
        return firstAnchor ? firstAnchor.getAttribute('href') : null;
    });";
            var urls = await page.EvaluateExpressionAsync<string[]>(links);

            foreach (string s in urls)
            {
                programmerLinks.Add("https://en.wikipedia.org" + s);
            }

            ViewBag.wikilinks = programmerLinks;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}