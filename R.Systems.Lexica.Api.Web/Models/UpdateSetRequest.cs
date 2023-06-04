namespace R.Systems.Lexica.Api.Web.Models;

public class UpdateSetRequest
{
    public long SetId { get; init; }

    public string SetName { get; init; } = "";

    public List<UpdateSetEntryRequest> Entries { get; set; } = new();
}

public class UpdateSetEntryRequest
{
    public string Word { get; set; } = "";

    public string WordType { get; set; } = "";

    public List<string> Translations { get; set; } = new();
}
