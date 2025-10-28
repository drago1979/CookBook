using Khaoticen.CookBook.Api.Core.Exceptions;
using Khaoticen.CookBook.Api.Core.Exceptions.Base;

namespace Khaoticen.CookBook.Api.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
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

    private static Task HandleDomainExceptionAsync(HttpContext context, DomainException ex)
    {
        var statusCode = ex switch
        {
            ValueNotAllowedException => 409,
            EntityNotFoundException => 404,
            InvalidRecipeException => 422,
            _ => 400
        };
        
        var result = new
        {
            status = statusCode,
            message = "Validation failed",
            errors = new Dictionary<string, string[]>
            {
                { "error", [ex.Message] }
            }
        };
        

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(result);
    }
}