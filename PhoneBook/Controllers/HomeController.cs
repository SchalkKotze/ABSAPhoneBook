using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PhoneBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration iconfig)
        {
            _logger = logger;
            configuration = iconfig;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
        public IActionResult Search(string searchString)
        {
            List<ViewPhoneBookModel> viewPhonebookList = new List<ViewPhoneBookModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(configuration.GetValue<string>("Route:BasePath") + $"/api/PhoneBookView/{searchString}"))
                {
                    var apiResponse = response.Result.Content.ReadAsStringAsync();
                    viewPhonebookList = JsonConvert.DeserializeObject<List<ViewPhoneBookModel>>(apiResponse.Result);
                }
            }

            return View(viewPhonebookList);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
