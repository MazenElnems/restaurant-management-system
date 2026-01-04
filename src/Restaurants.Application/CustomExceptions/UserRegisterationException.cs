namespace Restaurants.Application.CustomExceptions;

public class UserRegisterationException : Exception
{
    public UserRegisterationException(string message = "an error occured while regsitering the user")
        : base(message)
    {
        
    }
}
