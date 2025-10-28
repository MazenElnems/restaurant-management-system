using Microsoft.AspNetCore.Identity;

namespace Restaurants.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string? Nationality { get; set; }
    public DateOnly? DateOfBirth { get; set; }  
}
