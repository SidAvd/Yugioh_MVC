using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using Yugioh_MVC.Models;
using static Yugioh_MVC.Models.CardResults;
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
        private readonly IConfiguration _configuration;         // For appsettings.json info
        private readonly string _jsonFilePath;                  // Static JSON info to populate search form elements
        private readonly Uri _yugiohAPIBaseEndpoint;            // API's URI to fetch info
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

        // Starter View
        public IActionResult Index()
        {
            return View();
        }

        // Cards View
        public async Task<IActionResult> Cards(SearchCardInfo theModel)
        {
            if (theModel == null) return View();                                // If called with Model == null, passes empty table to the View
            var CardsSearchResultsList = await SearchCardsRequest(theModel);    // Search/fetch cards
            if (CardsSearchResultsList == null) return View();                  // If no results, passes empty table to the View
            ViewBag.NumberOfResults = CardsSearchResultsList.Length;            // Practicing ViewBag
            return View(CardsSearchResultsList.ToList());                       // Passing List of cards as Model to View
        }

        // SearchCards View
        public async Task<IActionResult> SearchCards()
        {
            ListPopulator listPopulator = await PopulateDropDownBoxesFromJsonFile();    // Static values populate form's Drop Down Lists
            if (listPopulator is null)
                return RedirectToAction("Error", "Home");
            ViewBag.AttributeList = listPopulator.attributes;                           // Pass info with the use of ViewBag
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
            return RedirectToAction("Cards","Home", new RouteValueDictionary(model));   // Call Home Controller's Cards method passing the model as argument
                                                                                        // model contains the user's search form choices
        }

        // Privacy View
        public IActionResult Privacy()
        {
            return View();
        }

        // Error View
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // Takes the user's search form choices, creates the final endpoint,
        // fetches the cards that match the endpoint, returns an Array of the cards results.
        async Task<CardInfo[]> SearchCardsRequest(SearchCardInfo searchCardInfo)
        {
            using (var myClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage myResponse = await myClient.GetAsync(CreateParameterEndpointString(searchCardInfo));
                    myResponse.EnsureSuccessStatusCode();
                    string myJson = await myResponse.Content.ReadAsStringAsync();
                    CardInfoArray theCardInfoArray = System.Text.Json.JsonSerializer.Deserialize<CardInfoArray>(myJson, _jsonOptions);
                    if (theCardInfoArray.data != null)
                        theCardInfoArray.data = theCardInfoArray.data.OrderBy(x => x.atk).ThenBy(x => x.def).ThenBy(x => x.name).ToArray(); // Practicing LINQ
                    return theCardInfoArray.data;
                }
                catch(HttpRequestException ex)
                {
                    _logger.LogInformation($"Error: {ex.Message}");
                    RedirectToAction("Error", "Home");
                    return null;
                }
            }
        }

        // Takes a SearchCardInfo object and returns the final endpoint string
        string CreateParameterEndpointString(SearchCardInfo searchCardInfo)
        {
            searchCardInfo.CreateEndpointDictionary();
            return searchCardInfo.ReturnEndpoint(_yugiohAPIBaseEndpoint);
        }

        // Reads static info from json file, that is used to populate the drop down lists in search form
        async Task<ListPopulator> PopulateDropDownBoxesFromJsonFile()
        {
            try
            {
                string jsonString = await System.IO.File.ReadAllTextAsync(_jsonFilePath);
                ListPopulator listPopulator = JsonConvert.DeserializeObject<ListPopulator>(jsonString);
                return listPopulator;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
