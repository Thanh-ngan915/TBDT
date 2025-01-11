﻿using System.ComponentModel.DataAnnotations;

namespace Shopping_Toturial.Models;

public class UserModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Hãy nhập username!")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Hãy nhập email!"), EmailAddress]
    public string Email { get; set; }
    
    [DataType(DataType.Password), Required(ErrorMessage = "Hãy nhập password!")]
    public string Password { get; set; }
}