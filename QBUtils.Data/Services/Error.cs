using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace QBUtils.Data.Services;

public abstract class Error
{
    public int StatusCode { get; set; }
    public required string StatusMessage { get; set; }
    public ModelStateDictionary ModelState { get; }

    public Error()
    {
        ModelState = new ModelStateDictionary();
    }

    public static BadRequestError BadRequestError() => new BadRequestError();
    public static BadRequestError BadRequestError(string key, string errorMessage) => new BadRequestError(key, errorMessage);
    public static ForbiddenError ForbiddenError() => new ForbiddenError();
    public static NotFoundError NotFoundError() => new NotFoundError();
    public static NotFoundError NotFoundError(string key, string value) => new NotFoundError($"No {key} with value '{value}' was found.");
    public static ServerError ServerError() => new ServerError();
}

public class BadRequestError : Error
{
    [SetsRequiredMembers]
    public BadRequestError() : this("Bad Request")
    {
    }

    [SetsRequiredMembers]
    public BadRequestError(string key, string errorMessage) : this()
    {
        ModelState.TryAddModelError(key, errorMessage);
    }

    [SetsRequiredMembers]
    public BadRequestError(string errorMessage) : base()
    {
        StatusCode = 400;
        StatusMessage = errorMessage;
    }
}

public class ForbiddenError : Error
{
    [SetsRequiredMembers]
    public ForbiddenError() : this("Forbidden")
    {

    }

    [SetsRequiredMembers]
    public ForbiddenError(string errorMessage) : base()
    {
        StatusCode = 403;
        StatusMessage = errorMessage;
    }
}

public class NotFoundError : Error
{
    [SetsRequiredMembers]
    public NotFoundError() : this("Not Found")
    {

    }

    [SetsRequiredMembers]
    public NotFoundError(string key, string errorMessage) : this()
    {
        ModelState.TryAddModelError(key, errorMessage);
    }

    [SetsRequiredMembers]
    public NotFoundError(string errorMessage) : base()
    {
        StatusCode = 404;
        StatusMessage = errorMessage;
    }
}

public class ServerError : Error
{
    [SetsRequiredMembers]
    public ServerError() : this("Server Error")
    {

    }

    [SetsRequiredMembers]
    public ServerError(string errorMessage) : base()
    {
        StatusCode = 500;
        StatusMessage = errorMessage;
    }
}
