using FluentResults;

namespace SelfResearch.Core.Infraestructure.ErrorHandling;

public class ArgumentError : Error
{
    public string PropertyName { get; }
    public ArgumentError(string propertyName, string message)
        : base($"Wrong argument value for '{propertyName}': {message}")
    {
        PropertyName = propertyName;
    }
}

public class NotFoundError : Error
{
    public NotFoundError(string identifier, string entityName)
        : base($"Entity '{entityName}' not found for given identifier: {identifier}") { }
}