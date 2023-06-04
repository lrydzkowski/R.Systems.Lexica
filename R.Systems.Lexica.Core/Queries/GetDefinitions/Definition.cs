namespace R.Systems.Lexica.Core.Queries.GetDefinitions;

public class Definition
{
    public string Word { get; init; } = "";

    public string Text { get; init; } = "";

    public List<string> ExampleUses { get; init; } = new();
}
