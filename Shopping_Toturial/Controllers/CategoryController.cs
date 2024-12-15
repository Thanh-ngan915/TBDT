using Microsoft.AspNetCore.Mvc;

namespace Shopping_Toturial.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
