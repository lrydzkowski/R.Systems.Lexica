namespace R.Systems.Lexica.Api.Web.Models;

public class SetDto
{
    public long SetId { get; init; }

    public string Name { get; init; } = "";

    public List<EntryDto> Entries { get; init; } = new();

    public DateTimeOffset CreatedAt { get; init; }
}

public class EntryDto
{
    public string Word { get; set; } = "";

    public string WordType { get; set; } = "";

    public List<string> Translations { get; set; } = new();
}
