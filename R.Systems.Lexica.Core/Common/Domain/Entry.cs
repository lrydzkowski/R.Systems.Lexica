namespace R.Systems.Lexica.Core.Common.Domain;

public class Entry
{
    public string Word { get; init; } = "";

    public WordType WordType { get; init; } = WordType.None;

    public List<string> Translations { get; init; } = new();
}
