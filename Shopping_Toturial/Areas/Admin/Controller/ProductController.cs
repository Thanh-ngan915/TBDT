using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shopping_Toturial.Models;
using Shopping_Toturial.Reponsitory;


namespace Shopping_Toturial.Areas.Admin.Controller;

[Area("Admin")]
[Authorize(Roles = "Admin")]  // chi co admin duoc vao trang nay

public class ProductController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly DataContext _dataContext;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)

    {
        _dataContext = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET
    public async Task<IActionResult> Index(int pg = 1)
    {
        List<ProductModel> product = await _dataContext.Products.OrderByDescending(p => p.Id).Include(p => p.Category)
            .Include(p => p.Brand).ToListAsync();

        const int pageSize = 10;

        if (pg < 1)
        {
            pg = 1;
        }
        int recsCount = product.Count();

        var pager = new Pagination(recsCount, pg, pageSize);

        int recSkip = (pg - 1) * pageSize;

        var data = product.Skip(recSkip).Take(pager.PageSize).ToList();

        ViewBag.Pager = pager;
        return View(data);
    }
    
    

    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
        ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductModel product)
    {
        ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
        ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

        if (ModelState.IsValid)
        {
            product.Slug = product.Name.Replace(" ", "-");
            var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Sản phẩm đã có trong database");
                return View(product);
            }
            else
            {
                if (product.Images != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Images = imageName;
                }
             }
            _dataContext.Add(product);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Sản phẩm thêm thành công";

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

        return View(product);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ProductModel product = await _dataContext.Products.FindAsync(id);
        ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
        ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
        return View(product);
    }

   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,ProductModel product)
    {
        ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
        ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
       var existed_product=_dataContext.Products.Find(product.Id); 
        
        if (ModelState.IsValid)
        {
            product.Slug = product.Name.Replace(" ", "-");
            var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Sản phẩm đã có trong database");
                return View(product);
            }
            else
            {
                if (product.Price != null)
                {
                    //upload new image
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    
                    string oldfilePath = Path.Combine(uploadDir, existed_product.Images);
                    try
                    {
                        if (System.IO.File.Exists(oldfilePath))
                        {
                            System.IO.File.Delete(oldfilePath);
                        }

                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("","An error occured while deleting the  product image");
                    }

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_product.Images = imageName;
           
                    
                   

                }
                existed_product.Name= product.Name;
                existed_product.Desciption = product.Desciption;
                existed_product.Price= product.Price;
                existed_product.CategoryId = product.CategoryId;
                existed_product.BrandId = product.BrandId;

            }

            _dataContext.Update(existed_product);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Cập nhật sản phẩm thành công";

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

        return View(product);
    }

    public async Task<IActionResult> Delete(int id)
    {
        ProductModel product = await _dataContext.Products.FindAsync(id);
        if (!string.Equals(product.Images, "noname.ipg"))
        {
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
            string oldfilePath = Path.Combine(uploadDir, product.Images);
            try
            {
                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }

            }
            catch (Exception e)
            {
               ModelState.AddModelError("","An error occured while deleting the  product image");
            }
            
        }
        _dataContext.Products.Remove(product);
        await _dataContext.SaveChangesAsync();
        TempData["error"] = "Sản phaảm đã xóa";
        return RedirectToAction("Index");
        

    }
   
}

