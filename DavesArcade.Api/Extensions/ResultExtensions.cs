using DavesArcade.Application.Results;

namespace DavesArcade.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return result.Error!.Type switch
        {
            ErrorType.NotFound => Results.NotFound(new 
            { 
                error = result.Error.Description, 
                code = result.Error.Code 
            }),
            ErrorType.Validation => Results.BadRequest(new 
            { 
                error = result.Error.Description, 
                code = result.Error.Code 
            }),
            ErrorType.Conflict => Results.Conflict(new 
            { 
                error = result.Error.Description, 
                code = result.Error.Code 
            }),
            _ => Results.Problem("An unexpected error occurred.")
        };
    }

    public static IResult ToCreatedResult<T>(this Result<T> result, Func<T, string> locationSelector)
    {
        if (result.IsSuccess)
        {
            return Results.Created(locationSelector(result.Value!), result.Value);
        }

        return result.ToHttpResult();
    }
}