using Microsoft.AspNetCore.Mvc;

namespace RestaurantBookingMVC.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
