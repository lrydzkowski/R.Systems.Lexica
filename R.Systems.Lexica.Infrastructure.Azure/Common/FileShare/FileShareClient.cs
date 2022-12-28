using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace R.Systems.Lexica.Infrastructure.Azure.Common.FileShare;

internal interface IFileShareClient
{
    Task<List<string>> GetFilePathsAsync(string rootDirectory = "/");

    Task<string?> GetFileContentAsync(string filePath, string rootDirectory = "/");

    Task<byte[]?> GetFileAsync(string filePath, string rootDirectory = "/");

    Task SaveFileAsync(byte[] file, string fileName, string rootDirectory = "/");
}

internal class FileShareClient : IFileShareClient
{
    public FileShareClient(ShareClient shareClient)
    {
        ShareClient = shareClient;
    }

    private ShareClient ShareClient { get; }

    public async Task<List<string>> GetFilePathsAsync(string rootDirectory = "/")
    {
        if (!await ShareClient.ExistsAsync())
        {
            throw new InvalidOperationException($"File share with name {ShareClient.Name} doesn't exist.");
        }

        List<string> filePaths = new();
        Queue<ShareDirectoryClient> remaining = new();
        ShareDirectoryClient directory = GetDirectoryClient(rootDirectory);
        remaining.Enqueue(directory);
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

                string filePath = string.IsNullOrEmpty(dir.Path)
                    ? $"/{item.Name}"
                    : $"/{dir.Path}/{item.Name}";
                filePaths.Add("/" + filePath.Replace(rootDirectory, ""));
            }
        }

        return filePaths;
    }

    public async Task<string?> GetFileContentAsync(string filePath, string rootDirectory = "/")
    {
        if (!await ShareClient.ExistsAsync())
        {
            throw new InvalidOperationException($"File share with name {ShareClient.Name} doesn't exist.");
        }

        ShareFileClient file = GetFileClient(filePath, rootDirectory);
        if (!await file.ExistsAsync())
        {
            return null;
        }

        ShareFileDownloadInfo download = await file.DownloadAsync();

        using StreamReader reader = new(download.Content);

        return await reader.ReadToEndAsync();
    }

    public async Task<byte[]?> GetFileAsync(string filePath, string rootDirectory = "/")
    {
        if (!await ShareClient.ExistsAsync())
        {
            throw new InvalidOperationException($"File share with name {ShareClient.Name} doesn't exist.");
        }

        ShareFileClient file = GetFileClient(filePath, rootDirectory);
        if (!await file.ExistsAsync())
        {
            return null;
        }

        ShareFileDownloadInfo download = await file.DownloadAsync();

        using MemoryStream memoryStream = new();
        await download.Content.CopyToAsync(memoryStream);

        return memoryStream.ToArray();
    }

    public async Task SaveFileAsync(byte[] file, string fileName, string rootDirectory = "/")
    {
        if (!await ShareClient.ExistsAsync())
        {
            throw new InvalidOperationException($"File share with name {ShareClient.Name} doesn't exist.");
        }

        ShareDirectoryClient directoryClient = GetDirectoryClient(rootDirectory);
        ShareFileClient fileClient = await directoryClient.CreateFileAsync(fileName, file.Length);
        await fileClient.UploadAsync(new MemoryStream(file));
    }

    private ShareFileClient GetFileClient(string filePath, string rootDirectory)
    {
        ShareDirectoryClient directory = GetDirectoryClient(filePath, rootDirectory);

        return directory.GetFileClient(Path.GetFileName(filePath));
    }

    private ShareDirectoryClient GetDirectoryClient(string filePath, string rootDirectory)
    {
        rootDirectory = ParseDirectoryPath(rootDirectory);
        string fileDirectory = ParseDirectoryPath(Path.GetDirectoryName(filePath) ?? "").TrimStart('/');

        return ShareClient.GetDirectoryClient(rootDirectory + fileDirectory);
    }

    private ShareDirectoryClient GetDirectoryClient(string rootDirectory)
    {
        return ShareClient.GetDirectoryClient(ParseDirectoryPath(rootDirectory).TrimStart('/'));
    }

    private string ParseDirectoryPath(string rootDirectory)
    {
        rootDirectory = rootDirectory.Trim().Replace('\\', '/');
        if (string.IsNullOrEmpty(rootDirectory) || rootDirectory == "/")
        {
            return "/";
        }

        if (rootDirectory[0] != '/')
        {
            rootDirectory = "/" + rootDirectory;
        }

        if (rootDirectory[^1] != '/')
        {
            rootDirectory += "/";
        }

        return rootDirectory;
    }
}
