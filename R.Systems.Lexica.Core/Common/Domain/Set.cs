namespace R.Systems.Lexica.Core.Common.Domain;

public class Set
{
    public long SetId { get; init; }

    public string Name { get; init; } = "";

    public List<Entry> Entries { get; init; } = new();

    public DateTimeOffset CreatedAt { get; init; }
}
