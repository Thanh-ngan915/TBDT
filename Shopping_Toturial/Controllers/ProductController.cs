using Microsoft.AspNetCore.Mvc;

namespace Shopping_Toturial.Controllers
{
    public class ProductController:Controller
    {
        public IActionResult Index() { return View(); }
        public IActionResult Details() { return View(); }
    }
}
