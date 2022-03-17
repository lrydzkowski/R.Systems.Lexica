using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using R.Systems.Lexica.Core.Common.Exceptions;

namespace R.Systems.Lexica.WebApi.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public ApiExceptionFilterAttribute()
    {
        ExceptionHandlers.Add(typeof(ValidationException), HandleValidationException);
    }

    private Dictionary<Type, Action<ExceptionContext>> ExceptionHandlers { get; } = new();

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (ExceptionHandlers.ContainsKey(type))
        {
            ExceptionHandlers[type].Invoke(context);
        }
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;
        context.Result = new BadRequestObjectResult(exception.Errors);
        context.ExceptionHandled = true;
    }
}
