namespace R.Systems.Lexica.Core.Common.Domain;

public class Entry
{
    public string Word { get; set; } = "";

    public WordType WordType { get; set; } = WordType.None;

    public List<string> Translations { get; set; } = new();
}
