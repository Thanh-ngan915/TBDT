using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Toturial.Models;
using Shopping_Toturial.Reponsitory;

namespace Shopping_Toturial.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;

        public CategoryController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(String Slug="")
        {
            CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
            if (category == null) return RedirectToAction("Index");
            var productsByCategory = _dataContext.Products.Where(c => c.CategoryId == category.Id);
            return View(await productsByCategory.OrderByDescending(c => c.Id).ToListAsync());
        }
    }
}
