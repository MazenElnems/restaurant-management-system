namespace Restaurants.Application.Options;

public class JWTOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public double LifeTimeInMinutes { get; set; }
    public string SignInKey { get; set; }
}
