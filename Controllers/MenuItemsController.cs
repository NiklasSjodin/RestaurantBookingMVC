using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingMVC.Models;

namespace RestaurantBookingMVC.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly HttpClient _httpClient;
        private string baseUri = "https://localhost:7037/"; // Vår bas url från vårt api
        public MenuItemsController(HttpClient client)
        {
            _httpClient = client;
        }
        public async Task <IActionResult> Index()
        {
            ViewData["Title"] = "Menu | BG's"; // Sätter titeln i fliken

            var response = await _httpClient.GetAsync($"{baseUri}api/MenuItems"); // Hämtar vår data från vårt API
            var json = await response.Content.ReadAsStringAsync(); // Omvandlar datan till json sträng
            var menuItemList = JsonConvert.DeserializeObject<List<MenuItem>>(json); // Omvandlar strängen till en list av menu item

            return View(menuItemList);
        }
    }
}
