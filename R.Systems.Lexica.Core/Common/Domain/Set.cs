namespace R.Systems.Lexica.Core.Common.Domain;

public class Set
{
    public string Path { get; init; } = "";

    public List<Entry> Entries { get; init; } = new();

    public DateTimeOffset? LastModified { get; init; }
}
