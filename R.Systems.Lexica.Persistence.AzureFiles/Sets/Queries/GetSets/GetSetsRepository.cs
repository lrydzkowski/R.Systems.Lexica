using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Common.Lists.Extensions;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using R.Systems.Lexica.Persistence.AzureFiles.Common.FileShare;

namespace R.Systems.Lexica.Persistence.AzureFiles.Sets.Queries.GetSets;

internal class GetSetsRepository : IGetSetsRepository
{
    public GetSetsRepository(IFileShareClient fileShareClient)
    {
        FileShareClient = fileShareClient;
    }

    private IFileShareClient FileShareClient { get; }

    public async Task<ListInfo<Set>> GetSetsAsync(ListParameters listParameters, bool includeSetContent)
    {
        List<string> fieldsAvailableToSort = new() { nameof(Set.Path) };
        List<string> fieldsAvailableToFilter = new() { nameof(Set.Path) };
        List<string> filePaths = await FileShareClient.GetFilePathsAsync();

        IQueryable<Set> query = filePaths
            .Select(
                filePath => new Set
                {
                    Path = filePath
                }
            )
            .AsQueryable()
            .Sort(fieldsAvailableToSort, listParameters.Sorting, nameof(Set.Path))
            .Filter(fieldsAvailableToFilter, listParameters.Search);

        int numberOfAllRows = query.Count();
        List<Set> sets = query
            .Paginate(listParameters.Pagination)
            .ToList();

        List<Set> parsedSets = await ParseSetsAsync(sets, includeSetContent);

        return new ListInfo<Set>
        {
            Data = parsedSets,
            NumberOfAllRows = numberOfAllRows
        };
    }

    private async Task<List<Set>> ParseSetsAsync(List<Set> setsToParse, bool includeSetContent)
    {
        if (!includeSetContent)
        {
            return setsToParse;
        }

        List<Set> sets = new();
        foreach (Set setToParse in setsToParse)
        {
            Set set = new()
            {
                Path = setToParse.Path,
                Entries = await GetSetEntriesAsync(setToParse.Path)
            };
            sets.Add(set);
        }

        return sets;
    }

    private async Task<List<Entry>> GetSetEntriesAsync(string filePath)
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
