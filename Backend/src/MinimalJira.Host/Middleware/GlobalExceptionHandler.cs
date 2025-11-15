using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MinimalJira.Domain.Exceptions;
using Npgsql;

namespace MinimalJira.Host.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails = exception switch
        {
            NotFoundException => new ProblemDetails()
            {
                Status = StatusCodes.Status404NotFound,
                Title = exception.GetType().ToString(),
                Detail = exception.Message,
                Instance = exception.Source,
                Type = exception.HelpLink
            },
            NpgsqlException => new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = exception.GetType().ToString(),
                Detail = "База данных недоступна",
                Instance = exception.Source,
                Type = exception.HelpLink
            },
            _ => new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = exception.GetType().ToString(),
                Detail = exception.Message,
                Instance = exception.Source,
                Type = exception.HelpLink
            }
        };

        httpContext.Response.StatusCode = (int)problemDetails.Status!;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}