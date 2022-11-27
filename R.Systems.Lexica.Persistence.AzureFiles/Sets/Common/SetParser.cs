using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Persistence.AzureFiles.Sets.Common;

internal class SetParser
{
    public List<Entry> ParseContent(string content)
    {
        List<Entry> entries = new();
        string[] lines = content.Split('\n');
        foreach (string line in lines)
        {
            string[] lineParts = line.Split(';');
            if (lineParts.Length != 2)
            {
                continue;
            }

            List<string> words = lineParts[0].Split(',').Select(x => x.Trim()).ToList();
            List<string> translations = lineParts[1].Split(',').Select(x => x.Trim()).ToList();
            Entry entry = new() { Words = words, Translations = translations };
            entries.Add(entry);
        }

        return entries;
    }
}
