using Microsoft.AspNetCore.Mvc;

namespace RestaurantBookingMVC.Controllers
{
    public class GiftCardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
