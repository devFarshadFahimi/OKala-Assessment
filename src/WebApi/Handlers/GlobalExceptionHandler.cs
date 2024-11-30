using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Handlers;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails result = new();
        switch (exception)
        {
            case InvalidCastException exp:
                {
                    result.Detail = exp.Message;
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                }
            case InvalidDataException exp:
                {
                    result.Detail = exp.Message;
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                }
            default:
                result.Detail = "Unexpected error...!";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;
        }


        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
        return true;
    }
}