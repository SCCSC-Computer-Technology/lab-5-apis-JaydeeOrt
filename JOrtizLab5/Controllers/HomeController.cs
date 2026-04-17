using JOrtizLab5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Cpt206.SqlServer;

namespace JOrtizLab5.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;
        private readonly NorthwindContext db;
        private readonly IHttpClientFactory clientFactory;
        public HomeController(ILogger<HomeController> logger, NorthwindContext _db, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            db = _db;//data
            clientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Customers(string country)
        {
            string uri;
            if (string.IsNullOrEmpty(country))
            {
                ViewData["Title"] = "All Customers WorldWide";
                uri = "api/customers/";
            }
            else
            {
                ViewData["Title"] = $"Customers in {country}";
                uri = $"api/customers/?Country={country}";
            }

            HttpClient client = clientFactory.CreateClient(name: "NorthwindWebApi");
            HttpRequestMessage requesst = new HttpRequestMessage(HttpMethod.Get, uri);

            HttpResponseMessage response = await client.SendAsync(requesst);

            IEnumerable<Customer>? model = await
                response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();
            return View(model);
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
