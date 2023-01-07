namespace R.Systems.Lexica.Infrastructure.Forvo.Common.Api;

public class ForvoApiException : Exception
{
    public ForvoApiException(string message) : base(message)
    {
    }

    public ForvoApiException(string message, Exception inner) : base(message, inner)
    {
    }
}
