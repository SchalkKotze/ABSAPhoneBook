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
    public class PhoneBookController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration configuration;

        public PhoneBookController(ILogger<HomeController> logger, IConfiguration iconfig)
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

        public IActionResult Maintain()
        {
            List<PhoneBookModel> phonebookList = new List<PhoneBookModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(configuration.GetValue<string>("Route:BasePath") + "/api/PhoneBook/"))
                {
                    var apiResponse = response.Result.Content.ReadAsStringAsync();
                    phonebookList = JsonConvert.DeserializeObject<List<PhoneBookModel>>(apiResponse.Result);
                }
            }
            return View(phonebookList);
        }
      

        public IActionResult AddPhoneBook(string phoneBookName)
        {
            using (var client = new HttpClient())
            {
                PhoneBookModel phonebookAdd = new PhoneBookModel();
                phonebookAdd.phonebookname = phoneBookName;

                var s = JsonConvert.SerializeObject(phonebookAdd);

                client.BaseAddress = new Uri(configuration.GetValue<string>("Route:BasePath"));

                var postTask = client.PostAsJsonAsync<PhoneBookModel>("/api/PhoneBook_Add/", phonebookAdd);

                postTask.Wait();

            }

            return View();
        }

        public IActionResult EditPhoneBook(int id, string phoneBookName)
        {
            using (var client = new HttpClient())
            {
                PhoneBookModel phonebookEdit = new PhoneBookModel();

                phonebookEdit.id = id;
                phonebookEdit.phonebookname = phoneBookName;

                var s = JsonConvert.SerializeObject(phonebookEdit);
                client.BaseAddress = new Uri(configuration.GetValue<string>("Route:BasePath"));
                var postTask = client.PostAsJsonAsync<PhoneBookModel>("/api/PhoneBook_Edit/", phonebookEdit);
                postTask.Wait();
            }

            PhoneBookModel phonebookEntry = new PhoneBookModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(configuration.GetValue<string>("Route:BasePath") + $"/api/PhoneBook/{id}"))
                {
                    var apiResponse = response.Result.Content.ReadAsStringAsync();
                    phonebookEntry = JsonConvert.DeserializeObject<PhoneBookModel>(apiResponse.Result);
                }
            }

            return View(phonebookEntry);
        }
      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
