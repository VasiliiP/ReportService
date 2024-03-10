using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ReportService.Web.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            await HandleValidationError(httpContext, validationException, cancellationToken);
            return true;
        }
        
        await HandleServerError(httpContext, exception, cancellationToken);
        return true;
    }

    private async Task HandleServerError(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error"
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }

    private async Task HandleValidationError(HttpContext httpContext,
        ValidationException validationException, CancellationToken cancellationToken)
    {
        _logger.LogWarning(validationException, "Validation error occurred: {Message}", validationException.Message);

        var validationProblemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation error",
            Detail = validationException.Message
        };

        httpContext.Response.StatusCode = validationProblemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(validationProblemDetails, cancellationToken);
    }
}