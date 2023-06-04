namespace R.Systems.Lexica.Infrastructure.Wordnik.Models;

internal class DefinitionDto
{
    public string Text { get; init; } = "";

    public string Word { get; init; } = "";

    public List<DefinitionExampleUsesDto> ExampleUses { get; init; } = new();
}

internal class DefinitionExampleUsesDto
{
    public string Text { get; init; } = "";
}
