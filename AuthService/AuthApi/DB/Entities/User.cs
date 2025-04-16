using Microsoft.AspNetCore.Identity;

namespace AuthApi.DB.Entities;

public class User : IdentityUser
{
    public string? Initials { get; set; }
    public string? Address { get; set;}
}
