namespace R.Systems.Lexica.Core.Models;

public class Set
{
    public string Name { get; set; } = "";

    public List<Entry> Entries { get; set; } = new();
}
