using Diwan.Shared.Results;

namespace Diwan.Host.Endpoints.Common;

public static class ResultExtensions
{
    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok();
        }

        return MapError(result.Error);
    }

    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return MapError(result.Error);
    }

    private static IResult MapError(Error error)
    {
        return error.Type switch
        {
            ErrorType.NotFound => Results.NotFound(new { error = error.Code, message = error.Description }),
            ErrorType.Validation => Results.BadRequest(new { error = error.Code, message = error.Description }),
            ErrorType.Conflict => Results.Conflict(new { error = error.Code, message = error.Description }),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Forbidden => Results.Forbid(),
            _ => Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: error.Code,
                detail: error.Description)
        };
    }
}
