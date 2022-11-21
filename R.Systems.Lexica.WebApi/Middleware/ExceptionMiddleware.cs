using FluentValidation;
using R.Systems.Lexica.Core.Common.Errors;
using System.Net;
using System.Text.Json;

namespace R.Systems.Lexica.WebApi.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<Program> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<Program> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException validationException)
        {
            await HandleValidationExceptionAsync(httpContext, validationException);
        }
        catch (NotFoundException notFoundException)
        {
            await HandleNotFoundExceptionAsync(httpContext, notFoundException);
        }
        catch (Exception exception)
        {
            _logger.LogError($"Something went wrong: {exception}");
            HandleException(httpContext);
        }
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException validationException)
    {
        IEnumerable<ErrorInfo> errors = validationException.Errors.Select(
                x => new ErrorInfo
                {
                    PropertyName = x.PropertyName,
                    ErrorMessage = x.ErrorMessage,
                    AttemptedValue = x.AttemptedValue,
                    ErrorCode = x.ErrorCode
                }
            )
            .AsEnumerable();
        string errorsSerialized = SerializeResponse(errors);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        await context.Response.WriteAsync(errorsSerialized);
    }

    private async Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException notFoundException)
    {
        string errorsSerialized = SerializeResponse(notFoundException.Errors);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsync(errorsSerialized);
    }

    private void HandleException(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    }

    private string SerializeResponse<T>(T data)
    {
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(data, jsonSerializerOptions);
    }
}
