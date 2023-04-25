namespace R.Systems.Lexica.Core.Common.Domain;

public class Entry
{
    public string Word { get; init; } = "";

    public WordTypes WordType { get; init; } = WordTypes.None;

    public List<string> Translations { get; init; } = new();
}
