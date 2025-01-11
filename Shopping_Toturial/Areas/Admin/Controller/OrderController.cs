using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shopping_Toturial.Reponsitory;

namespace Shopping_Toturial.Areas.Admin.Controller;
[Area("Admin")]
[Authorize(Roles = "Admin")]  // chi co admin duoc vao trang nay
public class OrderController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly DataContext _dataContext;

    public OrderController(DataContext context)

    {
        _dataContext = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
    }
    public async Task<IActionResult> ViewOrder(string orderCode)
    {
        var detailsOrder = await _dataContext.OrderDetails.Include(od=>od.Product).Where(od=>od.OrderCode==orderCode).ToListAsync();
        return View(detailsOrder);
    }
}