namespace R.Systems.Lexica.Infrastructure.EnglishDictionary.Errors;

public class EnglishDictionaryException : Exception
{
    public EnglishDictionaryException()
    {
    }

    public EnglishDictionaryException(string message) : base(message)
    {
    }

    public EnglishDictionaryException(string message, Exception? inner) : base(message, inner)
    {
    }
}
