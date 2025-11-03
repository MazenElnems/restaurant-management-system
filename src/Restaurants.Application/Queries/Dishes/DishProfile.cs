using AutoMapper;
using Restaurants.Application.Commands.Dishes.CreateDish;
using Restaurants.Application.DTOs.Dishes;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Queries.Dishes;

public class DishProfile : Profile
{
    public DishProfile()
    {
        CreateMap<CreateDishCommand, Dish>();
        
        CreateMap<Dish, GetAllDishesDto>();
        
        CreateMap<Dish, GetDishDto>();
    }
}
