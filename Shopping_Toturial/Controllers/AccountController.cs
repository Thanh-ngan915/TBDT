using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopping_Toturial.Models;
using Shopping_Toturial.Models.ViewModels;

namespace Shopping_Toturial.Controllers;

public class AccountController: Controller
{
    private UserManager<AppUserModel> _userManager; // quan ly user
    private SignInManager<AppUserModel> _signInManager; // quan ly dang nhap

    public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }  
    
    public IActionResult Login(string returnUrl = "/home")
    {
        return View(new LoginViewModel {ReturnUrl = returnUrl});     // tra ve username va pass ben loginViewModel
    }
    
    [HttpPost]

    /*public async Task<IActionResult>  Login(LoginViewModel loginViewModel)
    {
        if (ModelState.IsValid) // neu du lieu oke
        {
           Microsoft.AspNetCore.Identity.SignInResult result = 
               await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false); // dang nhap dong bo voi password
           if (result.Succeeded)
           {
               return Redirect(loginViewModel.ReturnUrl ?? "/home");
           }
           ModelState.AddModelError("", "Sai username hoặc password");
        } 
        return View(loginViewModel);
    }*/
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(loginViewModel.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại.");
                return View(loginViewModel);
            }

            // Nếu tài khoản chưa được kích hoạt
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Tài khoản của bạn chưa được kích hoạt.");
                return View(loginViewModel);
            }

            // Nếu tài khoản bị khóa
            if (user.LockoutEnabled && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa.");
                return View(loginViewModel);
            }

            // Thực hiện đăng nhập
            var result = await _signInManager.PasswordSignInAsync(
                loginViewModel.Username, 
                loginViewModel.Password, 
                isPersistent: false, 
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Redirect( "/home");
            }

            ModelState.AddModelError("", "Sai username hoặc password.");
        }

        return View(loginViewModel);
    }

    
    // xu li dang nhap tai khoan 
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    
    // xu li dang ki tai khoan - dang ki thanh cong - tra ve trang dang nhap
    public async Task<IActionResult>  Create(UserModel user)
    {
        if (ModelState.IsValid) // neu du lieu oke
        {
            AppUserModel newUser = new AppUserModel{UserName = user.Username, Email = user.Email};
            IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
            if (result.Succeeded)
            {
                newUser.EmailConfirmed = true; // tự động xác nhận email
                return Redirect($"/account/Login");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        } 
        return View(user);
    }

    public async Task<IActionResult> Logout(String returnUrl = "/home")
    {
     await _signInManager.SignOutAsync();
     return Redirect(returnUrl);
    }

}