namespace R.Systems.Lexica.Api.Web.Models;

public class CreateSetRequest
{
    public string SetName { get; init; } = "";

    public List<CreateSetEntryRequest> Entries { get; set; } = new();
}

public class CreateSetEntryRequest
{
    public string Word { get; set; } = "";

    public string WordType { get; set; } = "";

    public List<string> Translations { get; set; } = new();
}
