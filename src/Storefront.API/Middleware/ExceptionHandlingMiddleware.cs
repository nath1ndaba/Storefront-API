using System.Net;
using System.Text.Json;
using FluentValidation;
using Storefront.Application.Common.Exceptions;

namespace Storefront.API.Middleware;

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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new
        {
            success = false,
            message = exception.Message,
            errors = new List<string>()
        };

        switch (exception)
        {
            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new
                {
                    success = false,
                    message = "Validation failed",
                    errors = validationException.Errors.Select(e => e.ErrorMessage).ToList()
                };
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse = new
                {
                    success = false,
                    message = "An internal server error occurred",
                    errors = new List<string> { exception.Message }
                };
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(result);
    }
}
