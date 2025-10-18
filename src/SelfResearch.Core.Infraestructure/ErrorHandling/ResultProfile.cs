using FluentResults;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SelfResearch.Core.Infraestructure.ErrorHandling;

namespace SelfResearch.Core.Infraestructure.ErrorHandlers;

public sealed class ResultProfile : DefaultAspNetCoreResultEndpointProfile
{
    // We get here when the query result has problems (Result.Fail) and we need to return the corresponding error.
    public override ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context)
    {
        var error = context.Result.Errors.FirstOrDefault();
        return error?.GetType().Name switch
        {
            nameof(ArgumentError) => new BadRequestObjectResult(error.Message),
            nameof(NotFoundError) => new NotFoundObjectResult(error.Message),
            _ => new BadRequestObjectResult("Generic error occurred."),
        };
    }

    // We get here when the query result is successful. In theory, we should return nothing except the "200" status, however, we get here because of the hacky "WithSuccess" method, or more precisely because of the lack of necessary overloads in the "Result.Ok" method. Therefore, we return the object as if we were in TransformOkValueResultToActionResult.
    public override ActionResult TransformOkNoValueResultToActionResult(OkResultToActionResultTransformationContext<Result> context)
    {
        var success = context.Result.Successes.FirstOrDefault();
        return new OkObjectResult(success.Message);
    }

    // We get here when the query result is successful and we need to return an object.
    public override ActionResult TransformOkValueResultToActionResult<T>(OkResultToActionResultTransformationContext<Result<T>> context)
    {
        return new OkObjectResult(context.Result.Value);
    }
}