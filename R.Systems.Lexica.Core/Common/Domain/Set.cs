namespace R.Systems.Lexica.Core.Common.Domain;

public class Set
{
    public string Name { get; init; } = "";

    public List<Entry> Entries { get; init; } = new();

    public DateTimeOffset CreatedAt { get; init; }
}

public class Entry
{
    public List<string> Words { get; init; } = new();

    public WordTypes WordType { get; init; } = WordTypes.None;

    public List<string> Translations { get; init; } = new();
}
