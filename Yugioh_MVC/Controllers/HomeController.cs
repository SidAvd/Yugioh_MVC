using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using Yugioh_MVC.Models;
using static Yugioh_MVC.Models.CardInfo;
using System.Web;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.IO;
using Microsoft.Extensions.Configuration;


namespace Yugioh_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _jsonFilePath;
        private readonly Uri _yugiohAPIBaseEndpoint;
        private readonly JsonSerializerOptions _jsonOptions;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _yugiohAPIBaseEndpoint = _configuration.GetValue<Uri>("Uri");
            _jsonFilePath = _configuration.GetValue<string>("JsonFilePath");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        // Action to show cards View
        public async Task<IActionResult> Cards(SearchCardInfo theModel)
        {
            if (theModel == null) return View();
            var CardsSearchResultsList = await SearchCardsRequest(theModel);
            if (CardsSearchResultsList == null) return View();
            ViewBag.NumberOfResults = CardsSearchResultsList.Length; // Just to practice ViewBag
            return View(CardsSearchResultsList.ToList());
        }

        // Action to show SearchCards View
        public async Task<IActionResult> SearchCards()
        {
            ListPopulator listPopulator = await PopulateDropDownBoxesFromJsonFile();
            if (listPopulator is null)
                return RedirectToAction("Error", "Home");
            ViewBag.AttributeList = listPopulator.attributes;
            ViewBag.RaceList = listPopulator.races;
            ViewBag.CardTypesList = listPopulator.cardtypes;
            ViewBag.LevelList = listPopulator.levels;
            ViewBag.ArchetypeList = listPopulator.archetypes;
            var model = new SearchCardInfo();
            return View(model);
        }

        // Action when SearchCards View's form is submitted - passes the info to Cards View
        public IActionResult SearchCardsForm(SearchCardInfo model)
        {
            /*TempData["SearchInfoModel"] = JsonConvert.SerializeObject(model);*/
            return RedirectToAction("Cards","Home", new RouteValueDictionary(model));
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


        // Takes a string and searches for card names that contain this string,
        // return a List of cards
        async Task<Datum[]> SearchCardsRequest(SearchCardInfo searchCardInfo)
        {
            using (var myClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage myResponse = await myClient.GetAsync(CreateParameterEndpointString(searchCardInfo));
                    myResponse.EnsureSuccessStatusCode();
                    string myJson = await myResponse.Content.ReadAsStringAsync();
                    Rootobject theRootobject = System.Text.Json.JsonSerializer.Deserialize<Rootobject>(myJson, _jsonOptions);
                    if (theRootobject.data != null)
                        theRootobject.data = theRootobject.data.OrderBy(x => x.atk).ThenBy(x => x.def).ThenBy(x => x.name).ToArray(); // Just to practice LINQ
                    return theRootobject.data;
                }
                catch(HttpRequestException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    RedirectToAction("Error", "Home");
                }
                return null;
            }
        }

        // Takes a SearchCardInfo object and returns the final endpoint string
        string CreateParameterEndpointString(SearchCardInfo searchCardInfo)
        {
            searchCardInfo.CreateEndpointDictionary();
            return searchCardInfo.ReturnEndpoint(_yugiohAPIBaseEndpoint);
        }

        // Reads static info from file, that is used to populate the drop don lists in search form
        async Task<ListPopulator> PopulateDropDownBoxesFromJsonFile()
        {
            try
            {
                string jsonString = await System.IO.File.ReadAllTextAsync(_jsonFilePath);
                ListPopulator listPopulator = JsonConvert.DeserializeObject<ListPopulator>(jsonString);
                return listPopulator;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: Problem with reading and deserializing the JSON file. {e.Message}");
            }
            return null;
        }
    }
}
