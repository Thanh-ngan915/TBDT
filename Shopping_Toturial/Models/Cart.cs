using Microsoft.AspNetCore.Mvc;

namespace Shopping_Toturial.Models
{
	public class Cart:Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
