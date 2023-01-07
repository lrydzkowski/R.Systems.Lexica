namespace R.Systems.Lexica.Infrastructure.Pronunciation.Common.Api;

internal class PronunciationApiException : Exception
{
    public PronunciationApiException(string message) : base(message)
    {
    }

    public PronunciationApiException(string message, Exception inner) : base(message, inner)
    {
    }
}
