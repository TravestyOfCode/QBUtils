using System.Diagnostics.CodeAnalysis;

namespace QBUtils.Data.Services;

public class Result
{
    public int StatusCode { get; set; }
    public bool WasSuccessful => StatusCode >= 299 && StatusCode <= 299;
    public required string StatusMessage { get; init; }
    public List<Error>? Errors { get; set; }
    public bool HasErrors => Errors != null && Errors.Any();

    [SetsRequiredMembers]
    public Result(int statusCode, string statusMessage)
    {
        StatusCode = statusCode;
        StatusMessage = statusMessage;

    }

    [SetsRequiredMembers]
    public Result(int statusCode, string statusMessage, Error error) : this(statusCode, statusMessage)
    {
        Errors = [error];
    }

    public static implicit operator Result(Error error) => new Result(error.StatusCode, error.StatusMessage);
}

public class Result<T> : Result
{
    private T? _Value;
    public T? Value => WasSuccessful ? _Value : throw new InvalidOperationException("Unable to get Value of unsuccessful Result.");

    [SetsRequiredMembers]
    public Result(int statusCode, string statusMessage, T? value) : base(statusCode, statusMessage)
    {
        _Value = value;
    }

    public static implicit operator Result<T>(T value) => new Result<T>(200, "OK", value);
    public static implicit operator Result<T>(Error error) => new Result<T>(error.StatusCode, error.StatusMessage, default);
}
