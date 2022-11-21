using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using R.Systems.Lexica.Core.Common.Errors;

namespace R.Systems.Lexica.Persistence.AzureFiles.Common.FileShare;

internal interface IFileShareClient
{
    Task<List<string>> GetFilePathsAsync();

    Task<string> GetFileContentAsync(string filePath);
}

internal class FileShareClient : IFileShareClient
{
    public FileShareClient(ShareClient shareClient)
    {
        ShareClient = shareClient;
    }

    private ShareClient ShareClient { get; }

    public async Task<List<string>> GetFilePathsAsync()
    {
        if (!await ShareClient.ExistsAsync())
        {
            throw new InvalidOperationException($"File share with name {ShareClient.Name} doesn't exist.");
        }

        List<string> filePaths = new();
        Queue<ShareDirectoryClient> remaining = new();
        remaining.Enqueue(ShareClient.GetRootDirectoryClient());
        while (remaining.Count > 0)
        {
            ShareDirectoryClient dir = remaining.Dequeue();
            await foreach (ShareFileItem item in dir.GetFilesAndDirectoriesAsync())
            {
                if (item.IsDirectory)
                {
                    remaining.Enqueue(dir.GetSubdirectoryClient(item.Name));
                    continue;
                }

                string filePath = string.IsNullOrEmpty(dir.Path) ? $"/{item.Name}" : $"/{dir.Path}/{item.Name}";
                filePaths.Add(filePath);
            }
        }

        return filePaths;
    }

    public async Task<string> GetFileContentAsync(string filePath)
    {
        if (!await ShareClient.ExistsAsync())
        {
            throw new InvalidOperationException($"File share with name {ShareClient.Name} doesn't exist.");
        }

        ShareDirectoryClient directory =
            ShareClient.GetDirectoryClient(Path.GetDirectoryName(filePath)?.TrimStart('\\').TrimStart('/') ?? "");
        ShareFileClient file = directory.GetFileClient(Path.GetFileName(filePath));
        if (!await file.ExistsAsync())
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

        ShareFileDownloadInfo download = await file.DownloadAsync();

        using StreamReader reader = new(download.Content);
        string content = await reader.ReadToEndAsync();

        return content;
    }
}
