﻿namespace R.Systems.Lexica.Infrastructure.Wordnik.Models;

internal class WordnikApiException : Exception
{
    public WordnikApiException()
    {
    }

    public WordnikApiException(string message)
        : base(message)
    {
    }

    public WordnikApiException(string message, Exception? inner)
        : base(message, inner)
    {
    }
}
