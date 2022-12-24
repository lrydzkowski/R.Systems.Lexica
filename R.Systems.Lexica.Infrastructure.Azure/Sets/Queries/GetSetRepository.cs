using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Infrastructure.Azure.Common.FileShare;
using R.Systems.Lexica.Infrastructure.Azure.Sets.Common;

namespace R.Systems.Lexica.Infrastructure.Azure.Sets.Queries;

internal class GetSetRepository : IGetSetRepository
{
    public GetSetRepository(IFileShareClient fileShareClient, SetParser setParser)
    {
        FileShareClient = fileShareClient;
        SetParser = setParser;
    }

    private IFileShareClient FileShareClient { get; }
    private SetParser SetParser { get; }

    public async Task<List<Entry>> GetSetEntriesAsync(string filePath)
    {
        string setContent = await FileShareClient.GetFileContentAsync(filePath);

        return SetParser.ParseContent(setContent);
    }
}
