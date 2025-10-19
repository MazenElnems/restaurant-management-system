using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Dishes.DTOs;
using Restaurants.Application.Resaurants.DTOs;
using Restaurants.Application.Resaurants.Services;
using Restaurants.Application.Resaurants.Services.Interfaces;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantsService, RestaurantsService>();
        services.AddAutoMapper(conf =>
        {
            conf.CreateMap<Dish, DishDto>();

            conf.CreateMap<Category, CategoryDto>()
                .ForMember(dto => dto.Dishes, opt => opt.MapFrom(src => src.Dishes));

            conf.CreateMap<Restaurant, RestaurantDto>()
                .ForMember(dto => dto.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dto => dto.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dto => dto.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode))
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(src => src.Categories));

            conf.CreateMap<Restaurant, AllRestaurantsDto>()
                .ForMember(dto => dto.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dto => dto.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode))
                .ForMember(dto => dto.City, opt => opt.MapFrom(src => src.Address.City));
        });
        return services;
    }
}
