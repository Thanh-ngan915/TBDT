using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Toturial.Models;
using Shopping_Toturial.Reponsitory;

namespace Shopping_Toturial.Areas.Admin.Controller;

[Area("Admin")]
[Authorize(Roles = "Admin")]  // chi co admin duoc vao trang nay
public class BrandController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly DataContext _dataContext;

    public BrandController(DataContext context)

    {
        _dataContext = context;
    }
    public async Task<IActionResult> Index(int pg = 1)
    {
        List<BrandModel> brand = _dataContext.Brands.ToList();


        const int pageSize = 10;

        if (pg < 1)
        {
            pg = 1;
        }
        int recsCount = brand.Count();

        var pager = new Pagination(recsCount, pg, pageSize);

        int recSkip = (pg - 1) * pageSize;

        var data = brand.Skip(recSkip).Take(pager.PageSize).ToList();

        ViewBag.Pager = pager;

        return View(data);
    }
    
    public async Task<IActionResult> Edit(int Id)
    {
        BrandModel brand = await _dataContext.Brands.FindAsync(Id);
        return View(brand);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandModel brand)
    {
        if (ModelState.IsValid)
        {
            brand.Slug = brand.Name.Replace(" ", "-");
            var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Thương hiệu đã có trong database");
                return View(brand);
            }


            _dataContext.Add(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Thêm thương hiệu thành công";

            return RedirectToAction("Index");
        }
        else
        {
            TempData["error"] = "Model có một vài thứ đang bị lỗi";
            List<String> errors = new List<String>();
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            string errorMessage = String.Join("\n", errors);
            return BadRequest(errorMessage);
        }

        return View(brand);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BrandModel brand)
    {
        if (ModelState.IsValid)
        {
            brand.Slug = brand.Name.Replace(" ", "-");
            var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Thương hiệu đã có trong database");
                return View(brand);
            }


            _dataContext.Update(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Câp nhật thương hiệu thành công";

            return RedirectToAction("Index");
        }
        else
        {
            TempData["error"] = "Model có một vài thứ đang bị lỗi";
            List<String> errors = new List<String>();
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            string errorMessage = String.Join("\n", errors);
            return BadRequest(errorMessage);
        }

        return View(brand);
    }
    public async Task<IActionResult> Delete(int id)
    {
        BrandModel brand = await _dataContext.Brands.FindAsync(id);



        _dataContext.Brands.Remove(brand);
        await _dataContext.SaveChangesAsync();
        TempData["success"] = "Thương hiệu  đã xóa";
        return RedirectToAction("Index");
    }
}