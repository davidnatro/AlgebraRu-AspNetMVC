using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace mednik.Models;

public class User : IdentityUser
{
    public string Telegram { get; set; }
}