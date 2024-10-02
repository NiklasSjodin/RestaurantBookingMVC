using Microsoft.AspNetCore.Mvc;

namespace RestaurantBookingMVC.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Contact | BG's"; // Sätter titeln i fliken
            return View();
        }
    }
}
