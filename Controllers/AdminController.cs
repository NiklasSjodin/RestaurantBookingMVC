using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingMVC.Models;
using RestaurantBookingMVC.Models.DTOs;

namespace RestaurantBookingMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private string baseUri = "https://localhost:7037/"; // Vår bas url från vårt api
        public AdminController(HttpClient client)
        {
            _httpClient = client;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Admin | BG's"; // Sätter titeln i fliken
            var response = await _httpClient.GetAsync($"{baseUri}api/Reservations"); // Hämtar vår data från vårt API
            var json = await response.Content.ReadAsStringAsync(); // Omvandlar datan till json sträng
            var reservationsList = JsonConvert.DeserializeObject<List<ReservationDTO>>(json); // Omvandlar strängen till en list av menu item

            return View(reservationsList);
        }
    }
}
