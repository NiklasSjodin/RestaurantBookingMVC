using Microsoft.AspNetCore.Mvc;

namespace RestaurantBookingMVC.Controllers
{
    public class ChefsTableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
