using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Toturial.Models;
using Shopping_Toturial.Reponsitory;

namespace Shopping_Toturial.Controllers
{
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;

        public BrandController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(String Slug="")
        {
            BrandModel brand = _dataContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();
            if ( brand == null) return RedirectToAction("Index");
            var productsByBrand = _dataContext.Products.Where(c => c.BrandId == brand.Id);
            return View(await productsByBrand.OrderByDescending(c => c.Id).ToListAsync());
        }
    }
}