namespace Restaurants.Application.CustomExceptions;

public class ResourseNotFoundException : Exception
{
    public ResourseNotFoundException(string resourceName, string resourceIndetifier)
        : base($"Resource \"{resourceName}\" with identifier \"{resourceIndetifier}\" was not found.")
    {
        
    }
}
