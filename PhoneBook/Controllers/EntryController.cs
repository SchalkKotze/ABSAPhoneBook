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
    public class EntryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration configuration;

        public EntryController(ILogger<HomeController> logger, IConfiguration iconfig)
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


        public JsonResult ReturnPhoneBookJSONDataToAJax()
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
            var jsonData = phonebookList.ToList();                     
            return Json(jsonData);
        }

        public IActionResult MaintainEntry()
        {
            List<EntryModel> phonebookList = new List<EntryModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(configuration.GetValue<string>("Route:BasePath") + "/api/Entries/"))
                {
                    var apiResponse = response.Result.Content.ReadAsStringAsync();
                    phonebookList = JsonConvert.DeserializeObject<List<EntryModel>>(apiResponse.Result);
                }
            }
            return View(phonebookList);
        }

      

        public IActionResult EditEntry(int id, string name, string phonenumber, int phonebookid)
        {
            using (var client = new HttpClient())
            {
                EntryModel entryEdit = new EntryModel();

                entryEdit.id = id;
                entryEdit.name = name;
                entryEdit.phonenumber = phonenumber;
                entryEdit.phonebookid = phonebookid;

                client.BaseAddress = new Uri(configuration.GetValue<string>("Route:BasePath"));
                var postTask = client.PostAsJsonAsync<EntryModel>("/api/Entries_Edit/", entryEdit);
                postTask.Wait();
            }

            EntryModel entry = new EntryModel();
            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(configuration.GetValue<string>("Route:BasePath") + $"/api/Entries/{id}"))
                {
                    var apiResponse = response.Result.Content.ReadAsStringAsync();
                    entry = JsonConvert.DeserializeObject<EntryModel>(apiResponse.Result);
                }
            }

            return View(entry);
        }

        public IActionResult AddEntry(int id, string name, string phonenumber, int phonebookid)
        {
            using (var client = new HttpClient())
            {
                EntryModel entryAdd = new EntryModel();

                entryAdd.id = id;
                entryAdd.name = name;
                entryAdd.phonenumber = phonenumber;
                entryAdd.phonebookid = phonebookid;

                var s = JsonConvert.SerializeObject(entryAdd);

                client.BaseAddress = new Uri(configuration.GetValue<string>("Route:BasePath"));

                var postTask = client.PostAsJsonAsync<EntryModel>("/api/Entries_Add/", entryAdd);

                postTask.Wait();

            }

            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
