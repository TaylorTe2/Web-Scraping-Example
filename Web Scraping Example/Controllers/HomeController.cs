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
            string url = "https://www.ebay.com.au/itm/115649250531";

            List<string> programmerLinks = new List<string>();

            var options = new LaunchOptions()
            {
                Headless = true,
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"

            };

            var browser = await Puppeteer.LaunchAsync(options, null);
            var page = await browser.NewPageAsync();

            await page.GoToAsync(url);

            var values = @"Array.from(document.querySelectorAll('div.x-price-primary')).map(div => {
    const span = div.querySelector('span.ux-textspans');
    return span ? span.textContent : null;
});";


            var urls = await page.EvaluateExpressionAsync<string[]>(values);

            foreach (string s in urls)
            {
                programmerLinks.Add("price" + s);
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