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

        public  IActionResult Maintain()
        {
            List<PhoneBookModel> phonebookList = new List<PhoneBookModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response =  httpClient.GetAsync(configuration.GetValue<string>("Route:BasePath") +  "/api/PhoneBook/"))
                {
                    var apiResponse =  response.Result.Content.ReadAsStringAsync();
                    phonebookList = JsonConvert.DeserializeObject<List<PhoneBookModel>>(apiResponse.Result);
                }
            }
            return View(phonebookList);
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

        public IActionResult AddPhoneBook(string phoneBookName)
        {
            using (var client = new HttpClient())
            {
                PhoneBookModel phonebookAdd = new PhoneBookModel();
                phonebookAdd.phonebookname = phoneBookName;

                var s = JsonConvert.SerializeObject(phonebookAdd);

                client.BaseAddress = new Uri(configuration.GetValue<string>("Route:BasePath"));

                var postTask =  client.PostAsJsonAsync<PhoneBookModel>("/api/PhoneBook_Add/", phonebookAdd);
                
                postTask.Wait();

            }

            return View();
        }

        public IActionResult EditPhoneBook(int id ,string phoneBookName)
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
