using R.Systems.Lexica.Core.Common.Models;

namespace R.Systems.Lexica.Persistence.Files.Sets.Commands.CreateSet;

internal class SetSerializer
{
    public string Serialize(Set set)
    {
        var lines = set.Entries.ConvertAll(x => string.Join(",", x.Words) + ';' + string.Join(",", x.Translations));
        return string.Join("\n", lines);
    }
}
