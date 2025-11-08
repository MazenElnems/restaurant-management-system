namespace Restaurants.Application.CustomExceptions;

public class UnAuthenticatedException : Exception
{
    public UnAuthenticatedException(string message = "User is not authenticated")
        :base(message)
    {
        
    }
}
