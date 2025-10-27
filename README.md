# Restaurant Management System (RMS)

## Technologies & Tools

### Backend Framework
* ASP.NET Core Web API (.NET 9)
* C# 12

### Database & ORM
* SQL Server
* Entity Framework Core
* LINQ

### Architecture & Patterns
* Clean Architecture (Domain, Application, Infrastructure, API Layers)
* Repository Pattern
* Dependency Injection
* CQRS (Command Query Responsibility Segregation)

### Libraries & Packages
* AutoMapper - Object-to-object mapping
* FluentValidation - Input validation
* MediatR - Mediator pattern implementation
* Swagger/OpenAPI - API documentation

## Overview
The Restaurant Management System (RMS) is a backend API for managing restaurants, menus, and dishes. It enables restaurant owners, managers, and staff to organize restaurants, categories, and dishes efficiently. The system follows a clean and scalable architecture built with ASP.NET Core Web API and Entity Framework Core.

## Entities and Relationships

### Restaurant
**Properties**: Id, Name, Description, Address, Phone, Email, LogoUrl, IsActive, CreatedAt, UpdatedAt

### Category
**Properties**: Id, Name, Description, DisplayOrder, RestaurantId, CreatedAt, UpdatedAt

### Dish
**Properties**: Id, Name, Description, Price, ImageUrl, IsAvailable, Ingredients, RestaurantId, CategoryId, CreatedAt, UpdatedAt

### Staff
**Properties**: Id, UserId, RestaurantId, Role, AccessLevel, IsActive, JoinedAt, UpdatedAt

### User
**Properties**: Id, Username, Email, PasswordHash, FirstName, LastName, CreatedAt, UpdatedAt

## Features

### Restaurant Management
* Create, view, update, and delete restaurant profiles.
* Upload restaurant logo or images.
* Retrieve restaurant statistics (total dishes, categories, staff).

### Category Management
* Add, edit, delete, and reorder menu categories.
* Retrieve all categories in a specific restaurant.

### Dish Management
* Add, edit, delete, and view dishes for each restaurant.
* Move dishes between categories.
* Retrieve dishes by category or full restaurant menu.

### Staff Management
* Add, update, or remove restaurant staff.
* Manage staff roles and access permissions.

## Functionalities by Role

### Restaurant Manager
* Create and edit restaurant profiles.
* Add, edit, and delete categories and dishes.
* Move dishes between categories.
* View all dishes and categories.
* View restaurant summary (total dishes, categories, staff).

### Staff
* View all dishes and categories.
* Check dish details.
* Access assigned restaurant menu.

### Administrator
* Manage all restaurants on the system.
* Delete or suspend restaurants.
* Manage staff assignments and permissions.
* Control restaurant visibility (publish/unpublish).

## API Endpoints

### Restaurants
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/restaurants` | Get all restaurants |
| GET | `/api/restaurants/{id}` | Get a specific restaurant |
| POST | `/api/restaurants` | Create a new restaurant |
| PUT | `/api/restaurants/{id}` | Update a restaurant |
| DELETE | `/api/restaurants/{id}` | Delete a restaurant |
| GET | `/api/restaurants/{id}/summary` | Get restaurant statistics |
| POST | `/api/restaurants/{id}/upload-logo` | Upload restaurant logo |

### Categories
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/restaurants/{restaurantId}/categories` | Get all categories in a restaurant |
| GET | `/api/restaurants/{restaurantId}/categories/{id}` | Get category details |
| POST | `/api/restaurants/{restaurantId}/categories` | Create a new category |
| PUT | `/api/restaurants/{restaurantId}/categories/{id}` | Update a category |
| DELETE | `/api/restaurants/{restaurantId}/categories/{id}` | Delete a category |
| PATCH | `/api/restaurants/{restaurantId}/categories/reorder` | Reorder categories |

### Dishes
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/restaurants/{restaurantId}/dishes` | Get all dishes in a restaurant |
| GET | `/api/restaurants/{restaurantId}/dishes/{id}` | Get specific dish |
| POST | `/api/restaurants/{restaurantId}/dishes` | Create a new dish |
| PUT | `/api/restaurants/{restaurantId}/dishes/{id}` | Update a dish |
| DELETE | `/api/restaurants/{restaurantId}/dishes/{id}` | Delete a dish |
| PATCH | `/api/restaurants/{restaurantId}/dishes/{id}/move` | Move dish to another category |
| GET | `/api/restaurants/{restaurantId}/categories/{categoryId}/dishes` | Get dishes by category |

### Staff
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/restaurants/{restaurantId}/staff` | Get all staff members |
| POST | `/api/restaurants/{restaurantId}/staff` | Add new staff |
| PUT | `/api/restaurants/{restaurantId}/staff/{id}` | Update staff role |
| DELETE | `/api/restaurants/{restaurantId}/staff/{id}` | Remove staff |
