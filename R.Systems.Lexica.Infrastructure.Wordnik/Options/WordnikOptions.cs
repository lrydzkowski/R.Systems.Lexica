namespace R.Systems.Lexica.Infrastructure.Wordnik.Options;

internal class WordnikOptions
{
    public const string Position = "Wordnik";

    public string ApiBaseUrl { get; init; } = "";

    public string DefinitionsUrl { get; init; } = "";

    public string ApiKey { get; init; } = "";
}
