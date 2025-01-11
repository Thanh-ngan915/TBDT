using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Shopping_Toturial.Models;
using Shopping_Toturial.Reponsitory;

namespace Shopping_Toturial.Controllers;

public class CheckoutController : Controller
{
    private readonly DataContext _dataContext;
    public CheckoutController(DataContext _context)
    {
        _dataContext = _context;
    }

    public async Task<IActionResult> checkout()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (userEmail == null)
        {
            return RedirectToAction("Login", "Account");
        }
        else
        {
            var orderCode = Guid.NewGuid().ToString(); // random tao ra code
            var orderItem = new OrderModel();
            orderItem.OrderCode = orderCode;
            orderItem.Username = userEmail;
            orderItem.status = 1;
            orderItem.OrderDate = DateTime.Now;
            _dataContext.Add(orderItem);
            _dataContext.SaveChanges();
            List<CartItemModel> cartItem =
            HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            foreach (var cart in cartItem)
            {
                var orderDetails = new OrderDetails();
                orderDetails.Username = userEmail;
                orderDetails.OrderCode = orderCode;
                orderDetails.ProductId = cart.ProductId;
                orderDetails.Quantity = cart.Quantity;
                orderDetails.Price = cart.Price;
                _dataContext.Add(orderDetails);
                _dataContext.SaveChanges();
            }
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index", "Cart");

        }
        return Redirect( "Cart");
    }
}