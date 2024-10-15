using Microsoft.AspNetCore.Mvc;

namespace JobServicePortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Homepage()
        {
            return View();
        }
       

        public IActionResult About()
        {
            return View();
        }
    }
}
