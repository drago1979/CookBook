using System.Net;
using System.Text.Json;
using Khaoticen.CookBook.Api.Core.Exceptions;

namespace Khaoticen.CookBook.Api.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Not found: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex.Message, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
            await HandleExceptionAsync(context, "An unexpected error occurred.", HttpStatusCode.InternalServerError);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, string message, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = new
        {
            type = "https://tools.ietf.org/html/rfc9110#section-15.5",
            title = message,
            status = (int)statusCode,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}