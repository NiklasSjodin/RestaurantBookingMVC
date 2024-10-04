using Microsoft.AspNetCore.Mvc;

namespace RestaurantBookingMVC.Controllers
{
    public class BarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
