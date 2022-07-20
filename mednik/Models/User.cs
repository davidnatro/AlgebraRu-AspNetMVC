using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace mednik.Models;

public class User : IdentityUser
{
    public string? FullName { get; set; }
    
    public string? ImgUrl { get; set; }
    
    public string? Telegram { get; set; }
}