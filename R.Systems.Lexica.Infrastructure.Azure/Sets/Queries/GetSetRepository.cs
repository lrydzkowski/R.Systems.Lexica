using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Errors;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Infrastructure.Azure.Common.FileShare;
using R.Systems.Lexica.Infrastructure.Azure.Common.Options;
using R.Systems.Lexica.Infrastructure.Azure.Sets.Common;

namespace R.Systems.Lexica.Infrastructure.Azure.Sets.Queries;

internal class GetSetRepository : IGetSetRepository
{
    public GetSetRepository(IOptions<AzureFilesOptions> options, IFileShareClient fileShareClient, SetParser setParser)
    {
        Options = options.Value;
        FileShareClient = fileShareClient;
        SetParser = setParser;
    }

    private AzureFilesOptions Options { get; }
    private IFileShareClient FileShareClient { get; }
    private SetParser SetParser { get; }

    public async Task<List<Entry>> GetSetEntriesAsync(string filePath)
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
