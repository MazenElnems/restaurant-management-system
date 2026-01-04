using AutoMapper;
using Restaurants.Application.Commands.Auth.RegisterUserCommands;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.DTOs.Auth;

public class AuthModel
{
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}

public class AuthModelProfile : Profile
{
    public AuthModelProfile()
    {
        CreateMap<RegisterUserCommand, ApplicationUser>()
            .ForMember(entity => entity.Nationality, opt => opt.MapFrom(src => src.Country));
    }
}