namespace Restaurants.Domain.Common.Errors;

public static class AuthErrors
{
    public static Error InvalidEmailOrPassword = new($"{nameof(AuthErrors)}.InvalidEmailOrPassword", "The email or password provided is incorrect.");
    public static Error EmailAlreadyExists = new($"{nameof(AuthErrors)}.EmailAlreadyExists", "The email provided is already registered.");
    public static Error UserNameTaken = new($"{nameof(AuthErrors)}.UserNameTaken", "The username provided is already taken.");
}
