namespace R.Systems.Lexica.Core.Sets.Commands.CreateSet;

public class CreateSetEntryCommand
{
    public List<string> Words { get; init; } = new();

    public List<string> Translations { get; init; } = new();
}
