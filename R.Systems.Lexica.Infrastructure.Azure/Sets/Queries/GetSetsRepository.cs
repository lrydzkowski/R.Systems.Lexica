using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Errors;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Common.Lists.Extensions;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using R.Systems.Lexica.Infrastructure.Azure.Common.FileShare;
using R.Systems.Lexica.Infrastructure.Azure.Common.Options;
using R.Systems.Lexica.Infrastructure.Azure.Sets.Common;

namespace R.Systems.Lexica.Infrastructure.Azure.Sets.Queries;

internal class GetSetsRepository : IGetSetsRepository
{
    public GetSetsRepository(IOptions<AzureFilesOptions> options, IFileShareClient fileShareClient, SetParser setParser)
    {
        Options = options.Value;
        FileShareClient = fileShareClient;
        SetParser = setParser;
    }

    private AzureFilesOptions Options { get; }
    private IFileShareClient FileShareClient { get; }
    private SetParser SetParser { get; }

    public async Task<ListInfo<Set>> GetSetsAsync(ListParameters listParameters, bool includeSetContent)
    {
        List<string> fieldsAvailableToSort = new() { nameof(Set.Path) };
        List<string> fieldsAvailableToFilter = new() { nameof(Set.Path) };
        List<string> filePaths = await FileShareClient.GetFilePathsAsync(Options.SetsDirectoryPath);

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

        int count = query.Count();
        List<Set> sets = query
            .Paginate(listParameters.Pagination)
            .ToList();

        List<Set> parsedSets = await ParseSetsAsync(sets, includeSetContent);

        return new ListInfo<Set>
        {
            Data = parsedSets,
            Count = count
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
        string? setContent = await FileShareClient.GetFileContentAsync(filePath, Options.SetsDirectoryPath);
        if (setContent == null)
        {
            string errorMessage = $"File {filePath} doesn't exist.";
            throw new NotFoundException(
                errorMessage,
                new ErrorInfo
                {
                    PropertyName = "File",
                    AttemptedValue = filePath,
                    ErrorCode = "FileNotFound",
                    ErrorMessage = errorMessage
                }
            );
        }

        return SetParser.ParseContent(setContent);
    }
}
