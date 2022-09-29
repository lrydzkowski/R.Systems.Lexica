namespace R.Systems.Lexica.Core.Common.Domain;

public class Set
{
    public string Name { get; init; } = "";

    public List<Entry> Entries { get; init; } = new();
}
