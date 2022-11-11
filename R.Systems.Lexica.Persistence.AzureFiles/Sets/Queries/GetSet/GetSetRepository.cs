using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Persistence.AzureFiles.Common.FileShare;

namespace R.Systems.Lexica.Persistence.AzureFiles.Sets.Queries.GetSet;

internal class GetSetRepository : IGetSetRepository
{
    public GetSetRepository(IFileShareClient fileShareClient)
    {
        FileShareClient = fileShareClient;
    }

    private IFileShareClient FileShareClient { get; }

    public async Task<List<Entry>> GetSetEntriesAsync(string filePath)
    {
        string setContent = await FileShareClient.GetFileContentAsync(filePath);

        return ParseContent(setContent);
    }

    private List<Entry> ParseContent(string content)
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
