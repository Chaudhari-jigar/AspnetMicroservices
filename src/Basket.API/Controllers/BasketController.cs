using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
