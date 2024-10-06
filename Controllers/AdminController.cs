using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingMVC.Models;
using RestaurantBookingMVC.Models.DTOs;
using System.Text;

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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(ReservationDTO reservation)
        {
            if (!ModelState.IsValid)
            {
                return View(reservation);
            }

            var json = JsonConvert.SerializeObject(reservation); // Omvandlar reservation objektet till en sträng
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // Paketerar json strängen till en del av HTTP-förfrågan
            var response = await _httpClient.PostAsync($"{baseUri}api/Reservations/createReservation", content); // Vilken endpoint vi ska skicka det till och vad som ska skickas med
            return RedirectToAction("Index"); // Redirect till våran index sida om allt lyckas
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
        [HttpDelete]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseUri}api/Reservations/deleteReservation/{id}");

            return RedirectToAction("Index");
        }
    }
}
