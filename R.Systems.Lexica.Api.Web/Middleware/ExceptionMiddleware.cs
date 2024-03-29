﻿using System.Net;
using System.Text.Json;
using FluentValidation;
using R.Systems.Lexica.Core.Common.Errors;

namespace R.Systems.Lexica.Api.Web.Middleware;

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
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Operation was cancelled");
            HandleException(httpContext);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong");
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
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        string errorsSerialized = JsonSerializer.Serialize(errors, jsonSerializerOptions);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        await context.Response.WriteAsync(errorsSerialized);
    }

    private void HandleException(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    }
}
