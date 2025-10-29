using Khaoticen.CookBook.Api.Core.Exceptions;
using Khaoticen.CookBook.Api.Core.Exceptions.Base;

namespace Khaoticen.CookBook.Api.Api.Middleware;

/// <summary>
/// Middleware responsible for handling exceptions that occur during the processing
/// of HTTP requests in the application. Provides centralized exception handling
/// for different types of exceptions, logging unhandled exceptions, and generating
/// appropriate HTTP responses.
/// </summary>
/// <remarks>
/// This middleware specifically catches and handles two types of exceptions:
/// - <see cref="DomainException"/>: Used for application-specific errors with custom response handling.
/// - <see cref="Exception"/>: Catches general unhandled exceptions, logs an error, and responds with a standard 500 status code.
/// The middleware is designed to improve error response consistency and provide detailed logging for unhandled exceptions.
/// </remarks>
/// <param name="next">The next delegate in the middleware pipeline used for processing HTTP requests.</param>
/// <param name="logger">The logger instance used for logging exception details and errors.</param>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    /// <summary>
    /// Processes an incoming HTTP context, handling any unhandled exceptions
    /// and transforming them into appropriate HTTP responses.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <returns>A task that represents the asynchronous operation of processing the request.</returns>
    /// <remarks>
    /// This method catches specific domain exceptions defined in the application (e.g., ValueNotAllowedException,
    /// EntityNotFoundException, InvalidRecipeException) and returns appropriate HTTP status codes and error responses.
    /// For other unhandled exceptions, it logs the error and returns a 500 Internal Server Error response.
    /// </remarks>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            await HandleDomainExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            
            var result = new
            {
                status = 500,
                message = "Internal server error",
                errors = new { }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(result);
        }
    }

    /// <summary>
    /// Handles a domain-specific exception by setting an appropriate HTTP status code and response content.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <param name="ex">The domain exception to handle.</param>
    /// <returns>A task representing the asynchronous operation of writing the error response to the HTTP context.</returns>
    /// <remarks>
    /// This method maps specific domain exceptions to corresponding HTTP status codes and error messages:
    /// - ValueNotAllowedException: 409 Conflict with "Validation failed" message.
    /// - EntityNotFoundException: 404 Not Found with "Not found" message.
    /// - InvalidRecipeException: 422 Unprocessable Entity with "Invalid recipe" message.
    /// All other domain exceptions are mapped to 400 Bad Request with a "General error" message.
    /// The response includes the exception's message in the errors payload.
    /// </remarks>
    private static Task HandleDomainExceptionAsync(HttpContext context, DomainException ex)
    {
        var (statusCode, message) = ex switch
        {
            ValueNotAllowedException   => (409, "Validation failed"),
            EntityNotFoundException    => (404, "Not found"),
            InvalidRecipeException     => (422, "Invalid recipe"),
            _                          => (400, "General error")
        };

        var result = new
        {
            status = statusCode,
            message,
            errors = new Dictionary<string, string[]>
            {
                { "error", new[] { ex.Message } }
            }
        };
    
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(result);
    }

}