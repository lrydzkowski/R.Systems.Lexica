using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using R.Systems.Lexica.Core.Common.Errors;
using R.Systems.Lexica.Persistence.AzureFiles.Common.Models;

namespace R.Systems.Lexica.Persistence.AzureFiles.Common.FileShare;

internal interface IFileShareClient
{
    Task<List<AzureFileInfo>> GetFilesAsync(bool includeContent);

    Task<AzureFileInfo> GetFileAsync(string filePath, ShareDirectoryClient? directoryClient = null);
}

internal class FileShareClient : IFileShareClient
{
    public FileShareClient(ShareClient shareClient)
    {
        ShareClient = shareClient;
    }

    private ShareClient ShareClient { get; }

    public async Task<List<AzureFileInfo>> GetFilesAsync(bool includeContent)
    {
        await VerifyFileShareExistenceAsync();

        List<AzureFileInfo> files = new();
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
                ShareDirectoryClient directory = GetDirectoryClient(filePath);
                DateTimeOffset? lastModified;
                string? content = null;
                if (includeContent)
                {
                    AzureFileInfo fileInfo = await GetFileAsync(filePath, directory);
                    lastModified = fileInfo.LastModified;
                    content = fileInfo.Content;
                }
                else
                {
                    lastModified = await GetFileLastModifiedAsync(filePath, directory);
                }

                files.Add(
                    new AzureFileInfo()
                    {
                        FilePath = filePath,
                        LastModified = lastModified,
                        Content = content
                    }
                );
            }
        }

        return files;
    }

    public async Task<AzureFileInfo> GetFileAsync(string filePath, ShareDirectoryClient? directoryClient = null)
    {
        await VerifyFileShareExistenceAsync();

        (ShareFileClient file, ShareFileProperties shareFileProperties) =
            await GetShareFileAsync(filePath, directoryClient);

        ShareFileDownloadInfo download = await file.DownloadAsync();

        using StreamReader reader = new(download.Content);
        string content = await reader.ReadToEndAsync();

        return new AzureFileInfo
        {
            FilePath = filePath,
            Content = content,
            LastModified = shareFileProperties.LastModified
        };
    }

    private async Task VerifyFileShareExistenceAsync()
    {
        if (!await ShareClient.ExistsAsync())
        {
            throw new InvalidOperationException($"File share with name {ShareClient.Name} doesn't exist.");
        }
    }

    private async Task<DateTimeOffset> GetFileLastModifiedAsync(string filePath, ShareDirectoryClient? directory = null)
    {
        await VerifyFileShareExistenceAsync();

        (ShareFileClient _, ShareFileProperties shareFileProperties) = await GetShareFileAsync(filePath, directory);

        return shareFileProperties.LastModified;
    }

    private async Task<Tuple<ShareFileClient, ShareFileProperties>> GetShareFileAsync(
        string filePath,
        ShareDirectoryClient? directory = null
    )
    {
        directory ??= GetDirectoryClient(filePath);

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

        ShareFileProperties shareFileProperties = await file.GetPropertiesAsync();

        return new(file, shareFileProperties);
    }

    private ShareDirectoryClient GetDirectoryClient(string filePath)
    {
        return ShareClient.GetDirectoryClient(Path.GetDirectoryName(filePath)?.TrimStart('\\').TrimStart('/') ?? "");
    }
}
