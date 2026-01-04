using Restaurants.Domain.Common.Errors;

namespace Restaurants.Domain.Common;

public record Error(string Code, string? Description = null)
{
    public static readonly Error None = new Error(string.Empty);

    public static implicit operator Result(Error error) => Result.Failure(error);
}
