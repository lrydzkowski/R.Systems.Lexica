using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Common.Lists.Extensions;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using R.Systems.Lexica.Persistence.AzureFiles.Common.FileShare;
using R.Systems.Lexica.Persistence.AzureFiles.Common.Models;
using R.Systems.Lexica.Persistence.AzureFiles.Sets.Common;

namespace R.Systems.Lexica.Persistence.AzureFiles.Sets.Queries.GetSets;

internal class GetSetsRepository : IGetSetsRepository
{
    public GetSetsRepository(IFileShareClient fileShareClient, SetParser setParser)
    {
        FileShareClient = fileShareClient;
        SetParser = setParser;
    }

    private IFileShareClient FileShareClient { get; }
    private SetParser SetParser { get; }

    public async Task<ListInfo<Set>> GetSetsAsync(ListParameters listParameters, bool includeSetContent)
    {
        List<string> fieldsAvailableToSort = new() { nameof(Set.Path) };
        List<string> fieldsAvailableToFilter = new() { nameof(Set.Path) };
        List<AzureFileInfo> files = await FileShareClient.GetFilesAsync(includeSetContent);

        IQueryable<Set> query = files
            .Select(
                file => new Set
                {
                    Path = file.FilePath,
                    LastModified = file.LastModified,
                    Entries = file.Content == null ? new() : SetParser.ParseContent(file.Content)
                }
            )
            .AsQueryable()
            .Sort(fieldsAvailableToSort, listParameters.Sorting, nameof(Set.Path))
            .Filter(fieldsAvailableToFilter, listParameters.Search);

        int numberOfAllRows = query.Count();
        List<Set> sets = query
            .Paginate(listParameters.Pagination)
            .ToList();

        return new ListInfo<Set>
        {
            Data = sets,
            NumberOfAllRows = numberOfAllRows
        };
    }
}
