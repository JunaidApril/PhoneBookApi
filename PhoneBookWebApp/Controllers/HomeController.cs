using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhoneBook.DTO;
using PhoneBookWebApp.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhoneBookWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(PhoneContactsListViewModel phoneContactsList)
        {
            try
            {
                //var result = await GetAllContacts();

                return View(phoneContactsList);
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        public async Task<ActionResult> AddPhoneContact(string name, string cellphoneNumber, string homePhoneNumber, string workPhoneNumber)
        {
            try
            {
                var requestUri = "http://localhost:63136/api/PhoneBook/AddEntry";
                var parameters = $"?name={name}&cellphoneNumber={cellphoneNumber ?? ""}&homePhoneNumber={homePhoneNumber ?? ""}&workPhoneNumber={workPhoneNumber ?? ""}";

                RestClient client = new RestClient();
                RestRequest request = new RestRequest(requestUri + parameters, Method.POST);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = await client.ExecutePostAsync(request);

                //Return all contacts to the screen
                var result = await GetAllContacts();

                return View("Index", result);
            }
            catch(Exception ex)
            {
                return View("Index", new PhoneContactsListViewModel());
            }
        }

        public async Task<PhoneContactsListViewModel> GetAllContacts(string name = "")
        {
            try
            {
                var requestUri = "http://localhost:63136/api/PhoneBook/GetList";
                var parameters = $"?name={name ?? ""}";
                RestClient client = new RestClient();
                RestRequest request = new RestRequest(requestUri + parameters, Method.POST);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = await client.ExecuteGetAsync(request);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var phoneBookEntryDtoList = JsonConvert.DeserializeObject<List<PhoneContactResponseDto>>(response.Content);
                    if (phoneBookEntryDtoList != null && phoneBookEntryDtoList.Count > 0)
                    {
                        var phoneContactsListViewModel = new PhoneContactsListViewModel();
                        foreach (var x in phoneBookEntryDtoList)
                        {
                            var phoneContactsViewModel = new PhoneContactViewModel
                            {
                                Name = x.Name,
                                CellPhoneNumber = x.CellPhoneNumber,
                                HomePhoneNumber = x.HomePhoneNumber,
                                WorkPhoneNumber = x.WorkPhoneNumber
                            };
                            phoneContactsListViewModel.PhoneContacts.Add(phoneContactsViewModel);
                        }
                        return phoneContactsListViewModel;
                    } 
                }
                return new PhoneContactsListViewModel();
            }
            catch(Exception ex)
            {
                return new PhoneContactsListViewModel();
            }
        }

        public async Task<ActionResult> SearchPhoneContact(string name = "")
        {
            try
            {
                var result = await GetAllContacts(name);

                return View("Index", result);
            }
            catch(Exception ex)
            {
                return View("Index", new PhoneContactsListViewModel());
            }
        }
    }
}
