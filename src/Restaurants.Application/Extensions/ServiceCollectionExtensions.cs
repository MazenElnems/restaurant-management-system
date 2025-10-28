using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Commands.Categories.CreateCategory;
using Restaurants.Application.Commands.Restaurants.CraeteCommands;
using Restaurants.Application.DTOs.Categories;
using Restaurants.Application.DTOs.Dishes;
using Restaurants.Application.DTOs.Restaurants;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddAutoMapper(conf =>
        {
            conf.CreateMap<Category, GetAllCategoriesDto>();

            conf.CreateMap<Category, GetCategoryByIdDto>()
                .ForMember(dto => dto.DishIds, opt => opt.MapFrom(src => src.Dishes.Select(d => d.Id).ToList()));

            conf.CreateMap<CreateCategoryCommand, Category>();

            conf.CreateMap<Restaurant, GetRestaurantByIdDto>()
                .ForMember(dto => dto.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dto => dto.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dto => dto.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode));

            conf.CreateMap<Restaurant, GetAllRestaurantsDto>()
                .ForMember(dto => dto.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dto => dto.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode))
                .ForMember(dto => dto.City, opt => opt.MapFrom(src => src.Address.City));

            conf.CreateMap<CreateRestaurantCommand, Restaurant>()
                .ForMember(r => r.Address, opt => opt.MapFrom(dto => new Address
                {
                    City = dto.City,
                    Street = dto.Street,
                    PostalCode = dto.PostalCode,
                }));

        });
        return services;
    }
}
