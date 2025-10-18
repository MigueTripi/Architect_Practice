using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace SelfResearch.Core.Infraestructure.ErrorHandling;

public static class ResultExtensions
{
    /// <summary>
    /// Converts a FluentResults.Result to an ActionResult, allowing for a custom ObjectResult in case of success.
    /// Typical case is to return a 201 Created to stick REST conventions.
    /// </summary>
    /// <param name="result">The results</param>
    /// <param name="customObjectResult">The custom object result</param>
    /// <returns>The action result for the typed parameter</returns>
    public static ActionResult<T?> ToCustomActionResult<T>(this FluentResults.Result<T?> result, ObjectResult? customObjectResult = null)
    {
        if (result.IsSuccess && customObjectResult is not null)
        {
            return customObjectResult;
        }

        return result.ToActionResult();
    }
}
