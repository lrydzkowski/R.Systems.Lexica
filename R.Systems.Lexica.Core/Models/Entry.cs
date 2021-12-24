namespace R.Systems.Lexica.Core.Models;

public class Entry
{
    public List<string> Words { get; set; } = new();

    public List<string> Translations { get; set; } = new();
}
