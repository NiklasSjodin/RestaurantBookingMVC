using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingMVC.Models;
using RestaurantBookingMVC.Models.DTOs;
using System.Net.Http.Headers;
using System.Text;

namespace RestaurantBookingMVC.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private string baseUri = "https://localhost:7037/"; // Vår bas url från vårt api
        public AdminController(HttpClient client, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Api Client");
        }
        // Reservations
        public async Task<IActionResult> Index() // Hämtar alla reservationer
        {
            ViewData["Title"] = "Admin | BG's"; // Sätter titeln i fliken

            var response = await _httpClient.GetAsync($"{baseUri}api/Reservations"); // Hämtar vår data från vårt API
            var json = await response.Content.ReadAsStringAsync(); // Omvandlar datan till json sträng
            var reservationsList = JsonConvert.DeserializeObject<List<ReservationDTO>>(json); // Omvandlar strängen till en list av menu item

            return View(reservationsList);
        }

        public IActionResult CreateReservation()
        {
            ViewData["Title"] = "Create Reservation | BG's";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(ReservationCreate reservation)
        {
            if (!ModelState.IsValid)
            {
                return View(reservation);
            }

            var json = JsonConvert.SerializeObject(reservation);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{baseUri}api/Reservations/createReservation", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                // Om svaret innehåller ett specifikt felmeddelande (exempelvis för för många gäster)
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError("NumberOfGuests", errorMessage); // Lägg till felmeddelande för NumberOfGuests
                }
                else
                {
                    ModelState.AddModelError("", "Misslyckades med att skapa reservation."); // Generellt felmeddelande
                }

                return View(reservation); // Returnera samma vy med felmeddelande
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> UpdateReservation(int id)
        {
            var response = await _httpClient.GetAsync($"{baseUri}api/Reservations/reservation/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var reservation = JsonConvert.DeserializeObject<Reservation>(json);
            return View(reservation);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateReservation(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return View(reservation);
            }
            var json = JsonConvert.SerializeObject(reservation);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{baseUri}api/Reservations/updateReservation/{reservation.ReservationId}", content);
            if (!response.IsSuccessStatusCode)
            {
                // Logga eller hantera felet här
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating reservation: {errorMessage}");
                // Eventuellt kan du även returnera samma vy med reservationen för att visa felmeddelanden
                ModelState.AddModelError("", "Failed to update reservation.");
                return View(reservation);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseUri}api/Reservations/deleteReservation/{id}");

            return RedirectToAction("Index");
        }

        //------------------- Menu Items-------------------
        public async Task<IActionResult> MenuItems()
        {
            ViewData["Title"] = "Admin | BG's";
            var response = await _httpClient.GetAsync($"{baseUri}api/MenuItems");
            var json = await response.Content.ReadAsStringAsync();
            var menuItemsList = JsonConvert.DeserializeObject<List<MenuItem>>(json);

            return View(menuItemsList);
        }

        public IActionResult CreateMenuItem()
        {
            ViewData["Title"] = "Create Menu Item | BG's";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuItem(MenuItem item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{baseUri}api/MenuItems/createMenuItem", content);
            if (!response.IsSuccessStatusCode)
            {
                // Logga eller hantera felet här
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error creating menu item: {errorMessage}");
                // Eventuellt kan du även returnera samma vy med reservationen för att visa felmeddelanden
                ModelState.AddModelError("", "Failed to create menu item.");
                return View(item);
            }
            return RedirectToAction("MenuItems");
        }

        public async Task<IActionResult> UpdateMenuItem(int id)
        {
            var response = await _httpClient.GetAsync($"{baseUri}api/MenuItems/updateMenuItem/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var menuItem = JsonConvert.DeserializeObject<MenuItem>(json);

            return View(menuItem);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMenuItem(MenuItem item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{baseUri}api/MenuItems/updateMenuItem/{item.ItemId}", content);
            if (!response.IsSuccessStatusCode)
            {
                // Logga eller hantera felet här
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating menu item: {errorMessage}");
                // Eventuellt kan du även returnera samma vy med reservationen för att visa felmeddelanden
                ModelState.AddModelError("", "Failed to update menu item.");
                return View(item);
            }
            return RedirectToAction("MenuItems");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseUri}api/MenuItems/deleteMenuItem/{id}");

            return RedirectToAction("MenuItems");
        }

        //------------------- Tables -------------------

        public async Task<IActionResult> Tables()
        {
            ViewData["Title"] = "Admin | BG's";
            var token = HttpContext.Request.Cookies["jwtToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{baseUri}api/Tables");
            var json = await response.Content.ReadAsStringAsync();
            var tablesList = JsonConvert.DeserializeObject<List<Table>>(json);

            return View(tablesList);
        }

        public IActionResult CreateTable()
        {
            ViewData["Title"] = "Create Table | BG's";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable(Table table)
        {
            if (!ModelState.IsValid)
            {
                return View(table);
            }

            var json = JsonConvert.SerializeObject(table);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{baseUri}api/Tables/createTable", content);
            if (!response.IsSuccessStatusCode)
            {
                // Logga eller hantera felet här
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error creating table: {errorMessage}");
                // Eventuellt kan du även returnera samma vy med reservationen för att visa felmeddelanden
                ModelState.AddModelError("", "Failed to create table.");
                return View(table);
            }
            return RedirectToAction("Tables");
        }

        public async Task<IActionResult> UpdateTable(int id)
        {
            var response = await _httpClient.GetAsync($"{baseUri}api/Tables/updateTable/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var table = JsonConvert.DeserializeObject<MenuItem>(json);

            return View(table);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTable(Table table)
        {
            if (!ModelState.IsValid)
            {
                return View(table);
            }
            var json = JsonConvert.SerializeObject(table);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{baseUri}api/Tables/updateTable/{table.TableID}", content);
            if (!response.IsSuccessStatusCode)
            {
                // Logga eller hantera felet här
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating menu item: {errorMessage}");
                // Eventuellt kan du även returnera samma vy med reservationen för att visa felmeddelanden
                ModelState.AddModelError("", "Failed to update menu item.");
                return View(table);
            }
            return RedirectToAction("MenuItems");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseUri}api/Tables/deleteTable/{id}");

            return RedirectToAction("Tables");
        }

    }
}
