using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping_Toturial.Models;
using Shopping_Toturial.Reponsitory;

namespace Shopping_Toturial.Areas.Admin.Controller;

[Area("Admin")]
[Authorize(Roles = "Admin")]  // chi co admin duoc vao trang nay

public class CategoryController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly DataContext _dataContext;

    public CategoryController(DataContext context)

    {
        _dataContext = context;
    }

    public async Task<IActionResult> Index(int pg = 1)
    {
        List<CategoryModel> category = _dataContext.Categories.ToList(); //33 datas


        const int pageSize = 10; //10 items/trang

        if (pg < 1) //page < 1;
        {
            pg = 1; //page ==1
        }
        int recsCount = category.Count(); //33 items;

        var pager = new Pagination(recsCount, pg, pageSize);

        int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

        //category.Skip(20).Take(10).ToList()

        var data = category.Skip(recSkip).Take(pager.PageSize).ToList();

        ViewBag.Pager = pager;

        return View(data);
    }
    
    public async Task<IActionResult> Edit(int Id)
    {
        CategoryModel category = await _dataContext.Categories.FindAsync(Id);
        return View(category);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryModel category)
    {
        
        if (ModelState.IsValid)
        {
            category.Slug = category.Name.Replace(" ", "-");
            var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Danh mục đã có trong database");
                return View(category);
            }
          

            _dataContext.Add(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Thêm danh mục thành công";

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

        return View(category);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryModel category)
    {
        
        if (ModelState.IsValid)
        {
            category.Slug = category.Name.Replace(" ", "-");
            var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Danh mục đã có trong database");
                return View(category);
            }
          

            _dataContext.Update(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Cập nhật danh mục thành công";

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

        return View(category);
    }
    

   

    public async Task<IActionResult> Delete(int id)
    {
        CategoryModel category = await _dataContext.Categories.FindAsync(id);



        _dataContext.Categories.Remove(category);
        await _dataContext.SaveChangesAsync();
        TempData["success"] = "Danh mục đã xóa";
        return RedirectToAction("Index");
    }


}

