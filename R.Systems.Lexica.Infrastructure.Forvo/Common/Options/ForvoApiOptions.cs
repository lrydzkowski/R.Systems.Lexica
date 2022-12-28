namespace R.Systems.Lexica.Infrastructure.Forvo.Common.Options;

internal class ForvoApiOptions
{
    public const string Position = "ForvoApi";

    public string ApiKey { get; init; } = "";

    public string ApiUrl { get; init; } = "";
}
