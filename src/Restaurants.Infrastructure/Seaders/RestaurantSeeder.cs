using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Data;

namespace Restaurants.Infrastructure.Seaders;

internal class RestaurantSeeder : IRestaurantSeeder
{
    private readonly RestaurantDbContext _db;

    public RestaurantSeeder(RestaurantDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        if (await _db.Database.CanConnectAsync())
        {
            if (!_db.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                _db.Restaurants.AddRange(restaurants);
                await _db.SaveChangesAsync();
            }
        }
    }

    private List<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>
        {
            new Restaurant
            {
                Name = "La Bella Italia",
                Description = "Authentic Italian cuisine with traditional recipes passed down through generations",
                HasDelivery = true,
                ContactEmail = "info@labellaitalia.com",
                ContactNumber = "+1-555-0101",
                Address = new Address
                {
                    City = "New York",
                    Street = "125 Mulberry Street",
                    PostalCode = "10013"
                },
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Appetizers",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Bruschetta",
                                Description = "Grilled bread topped with fresh tomatoes, garlic, and basil",
                                Price = 8.99m
                            },
                            new Dish
                            {
                                Name = "Caprese Salad",
                                Description = "Fresh mozzarella, tomatoes, and basil with balsamic glaze",
                                Price = 12.99m
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Main Courses",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Spaghetti Carbonara",
                                Description = "Classic pasta with eggs, pecorino cheese, and pancetta",
                                Price = 18.99m
                            },
                            new Dish
                            {
                                Name = "Margherita Pizza",
                                Description = "Traditional pizza with tomato sauce, mozzarella, and fresh basil",
                                Price = 16.99m
                            },
                            new Dish
                            {
                                Name = "Osso Buco",
                                Description = "Braised veal shanks with vegetables and white wine",
                                Price = 28.99m
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Desserts",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Tiramisu",
                                Description = "Coffee-flavored Italian dessert with mascarpone cheese",
                                Price = 9.99m
                            },
                            new Dish
                            {
                                Name = "Panna Cotta",
                                Description = "Sweetened cream dessert with berry coulis",
                                Price = 8.99m
                            }
                        }
                    }
                }
            },
            new Restaurant
            {
                Name = "Dragon Palace",
                Description = "Exquisite Chinese cuisine featuring Cantonese and Szechuan specialties",
                HasDelivery = true,
                ContactEmail = "contact@dragonpalace.com",
                ContactNumber = "+1-555-0202",
                Address = new Address
                {
                    City = "San Francisco",
                    Street = "888 Grant Avenue",
                    PostalCode = "94108"
                },
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Dim Sum",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Har Gow",
                                Description = "Steamed shrimp dumplings with translucent wrapper",
                                Price = 7.99m
                            },
                            new Dish
                            {
                                Name = "Char Siu Bao",
                                Description = "Steamed BBQ pork buns with sweet filling",
                                Price = 6.99m
                            },
                            new Dish
                            {
                                Name = "Spring Rolls",
                                Description = "Crispy vegetable spring rolls with sweet chili sauce",
                                Price = 5.99m
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Main Dishes",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Kung Pao Chicken",
                                Description = "Spicy stir-fried chicken with peanuts and vegetables",
                                Price = 16.99m
                            },
                            new Dish
                            {
                                Name = "Sweet and Sour Pork",
                                Description = "Crispy pork with pineapple and bell peppers",
                                Price = 17.99m
                            },
                            new Dish
                            {
                                Name = "Beef with Broccoli",
                                Description = "Tender beef slices with fresh broccoli in oyster sauce",
                                Price = 18.99m
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Noodles & Rice",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Fried Rice",
                                Description = "Classic fried rice with eggs, vegetables, and choice of meat",
                                Price = 13.99m
                            },
                            new Dish
                            {
                                Name = "Lo Mein",
                                Description = "Soft egg noodles stir-fried with vegetables",
                                Price = 14.99m
                            }
                        }
                    }
                }
            },
            new Restaurant
            {
                Name = "Burger Haven",
                Description = "Gourmet burgers made with premium ingredients and house-made sauces",
                HasDelivery = true,
                ContactEmail = "hello@burgerhaven.com",
                ContactNumber = "+1-555-0303",
                Address = new Address
                {
                    City = "Chicago",
                    Street = "456 Michigan Avenue",
                    PostalCode = "60611"
                },
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Burgers",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Classic Cheeseburger",
                                Description = "Angus beef patty with cheddar, lettuce, tomato, and special sauce",
                                Price = 12.99m
                            },
                            new Dish
                            {
                                Name = "Bacon BBQ Burger",
                                Description = "Double patty with crispy bacon, BBQ sauce, and onion rings",
                                Price = 15.99m
                            },
                            new Dish
                            {
                                Name = "Mushroom Swiss Burger",
                                Description = "Beef patty with sautéed mushrooms and melted Swiss cheese",
                                Price = 14.99m
                            },
                            new Dish
                            {
                                Name = "Veggie Burger",
                                Description = "House-made black bean patty with avocado and sprouts",
                                Price = 11.99m
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Sides",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "French Fries",
                                Description = "Crispy golden fries with sea salt",
                                Price = 4.99m
                            },
                            new Dish
                            {
                                Name = "Onion Rings",
                                Description = "Beer-battered onion rings with ranch dipping sauce",
                                Price = 5.99m
                            },
                            new Dish
                            {
                                Name = "Coleslaw",
                                Description = "Creamy house-made coleslaw",
                                Price = 3.99m
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Shakes",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Chocolate Shake",
                                Description = "Rich chocolate milkshake topped with whipped cream",
                                Price = 6.99m
                            },
                            new Dish
                            {
                                Name = "Vanilla Shake",
                                Description = "Classic vanilla milkshake",
                                Price = 6.99m
                            }
                        }
                    }
                }
            },
            new Restaurant
            {
                Name = "Sushi Master",
                Description = "Premium Japanese restaurant offering fresh sushi and traditional dishes",
                HasDelivery = false,
                ContactEmail = "reservations@sushimaster.com",
                ContactNumber = "+1-555-0404",
                Address = new Address
                {
                    City = "Los Angeles",
                    Street = "789 Little Tokyo Street",
                    PostalCode = "90012"
                },
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Nigiri",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Salmon Nigiri",
                                Description = "Fresh Atlantic salmon over seasoned rice",
                                Price = 8.99m
                            },
                            new Dish
                            {
                                Name = "Tuna Nigiri",
                                Description = "Premium bluefin tuna over seasoned rice",
                                Price = 10.99m
                            },
                            new Dish
                            {
                                Name = "Eel Nigiri",
                                Description = "Grilled freshwater eel with sweet glaze",
                                Price = 9.99m
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Rolls",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "California Roll",
                                Description = "Crab, avocado, and cucumber wrapped in seaweed and rice",
                                Price = 11.99m
                            },
                            new Dish
                            {
                                Name = "Spicy Tuna Roll",
                                Description = "Spicy tuna with cucumber and sesame seeds",
                                Price = 13.99m
                            },
                            new Dish
                            {
                                Name = "Dragon Roll",
                                Description = "Shrimp tempura topped with avocado and eel sauce",
                                Price = 16.99m
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Hot Dishes",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Miso Soup",
                                Description = "Traditional soybean paste soup with tofu and seaweed",
                                Price = 4.99m
                            },
                            new Dish
                            {
                                Name = "Chicken Teriyaki",
                                Description = "Grilled chicken with teriyaki sauce and steamed vegetables",
                                Price = 17.99m
                            },
                            new Dish
                            {
                                Name = "Tempura Platter",
                                Description = "Assorted lightly battered and fried vegetables and shrimp",
                                Price = 19.99m
                            }
                        }
                    }
                }
            },
            new Restaurant
            {
                Name = "Taco Fiesta",
                Description = "Authentic Mexican street food with bold flavors and fresh ingredients",
                HasDelivery = true,
                ContactEmail = "orders@tacofiesta.com",
                ContactNumber = "+1-555-0505",
                Address = new Address
                {
                    City = "Austin",
                    Street = "321 Congress Avenue",
                    PostalCode = "78701"
                },
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Tacos",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Carne Asada Taco",
                                Description = "Grilled steak with cilantro, onions, and lime",
                                Price = 4.99m
                            },
                            new Dish
                            {
                                Name = "Al Pastor Taco",
                                Description = "Marinated pork with pineapple and traditional spices",
                                Price = 4.99m
                            },
                            new Dish
                            {
                                Name = "Fish Taco",
                                Description = "Beer-battered fish with cabbage slaw and chipotle mayo",
                                Price = 5.99m
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Burritos",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Bean and Cheese Burrito",
                                Description = "Refried beans with melted cheese in a flour tortilla",
                                Price = 8.99m
                            },
                            new Dish
                            {
                                Name = "Carnitas Burrito",
                                Description = "Slow-cooked pork with rice, beans, and guacamole",
                                Price = 12.99m
                            }
                        }
                    },
                    new Category
                    {
                        Name = "Sides",
                        Dishes = new List<Dish>
                        {
                            new Dish
                            {
                                Name = "Guacamole and Chips",
                                Description = "Fresh-made guacamole with crispy tortilla chips",
                                Price = 7.99m
                            },
                            new Dish
                            {
                                Name = "Queso Fundido",
                                Description = "Melted cheese dip with chorizo and jalapeños",
                                Price = 8.99m
                            },
                            new Dish
                            {
                                Name = "Elote",
                                Description = "Mexican street corn with mayo, cheese, and chili powder",
                                Price = 5.99m
                            }
                        }
                    }
                }
            }
        };
    }
}