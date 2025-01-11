using System.ComponentModel.DataAnnotations;

namespace Shopping_Toturial.Models.ViewModels;

// vì phần login ko có email nên tạo 1 model riêng chỉ có username và pass
public class LoginViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Hãy nhập username!")]
    public string Username { get; set; }
    
    [DataType(DataType.Password), Required(ErrorMessage = "Hãy nhập password!")]
    public string Password { get; set; }
    
    public string ReturnUrl { get; set; }
}
