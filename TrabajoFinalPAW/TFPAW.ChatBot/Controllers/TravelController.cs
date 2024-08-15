using Microsoft.AspNetCore.Mvc;

namespace TFPAW.Controllers
{
    public class TravelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Hoteles()
        {
            return View();
        }

        public IActionResult Playas()
        {
            return View();
        }

        public IActionResult Aventuras()
        {
            return View();
        }
    }
}
