using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Core.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : base("One or more validation failures have occurred.")
    {
    }

    public ValidationException(string? message) : base(message)
    {
    }

    public ValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public ValidationException(List<ErrorInfo> errors)
    {
        Errors = errors;
    }

    public ValidationException(ErrorInfo error)
    {
        Errors.Add(error);
    }

    public List<ErrorInfo> Errors { get; } = new();
}
