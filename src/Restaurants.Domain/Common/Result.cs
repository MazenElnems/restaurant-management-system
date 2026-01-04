namespace Restaurants.Domain.Common;

public class Result
{
    public Result(bool isSuccess, Error? error)
    {
        if(isSuccess && error != Error.None  || 
           !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid result state");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<T> : Result
{
    private readonly T? _value;
    private Result(bool isSuccess, T? value, Error? error)
        : base(isSuccess, error)
    {
        _value = value;
    }
    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("Cannot access the value of a failed result.");
            }
            return _value!;
        }
    }
    public static Result<T> Success(T value) => new(true, value, Error.None);
    public static new Result<T> Failure(Error error) => new(false, default, error);
}

