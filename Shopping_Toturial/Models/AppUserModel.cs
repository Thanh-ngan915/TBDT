using Microsoft.AspNetCore.Identity;

namespace Shopping_Toturial.Models;

public class AppUserModel: IdentityUser
{
    public string RoleId { get; set; }
}