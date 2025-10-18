using FluentResults;

namespace SelfResearch.Financial.API.Core.ErrorHandlers;

public abstract class GenericError : Error
{
    public string ErrorCode { get; }

    protected GenericError(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class ArgumentError : GenericError
{
    public string PropertyName { get; }
    public ArgumentError(string propertyName, string message)
        : base($"Wrong argument value for '{propertyName}': {message}", "422")
    {
        PropertyName = propertyName;
    }
}