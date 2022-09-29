namespace R.Systems.Lexica.Core.Common.Domain;

public class Entry
{
    public List<string> Words { get; init; } = new();

    public List<string> Translations { get; init; } = new();
}
