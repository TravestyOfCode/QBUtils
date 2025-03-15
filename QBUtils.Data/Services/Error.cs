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
