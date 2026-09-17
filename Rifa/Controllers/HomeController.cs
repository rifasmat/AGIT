using Microsoft.AspNetCore.Mvc;

namespace Rifa.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}