namespace R.Systems.Lexica.Infrastructure.Storage.Services;

internal interface IDirectoryHandler
{
    bool Exists(string directoryPath);
    DirectoryInfo CreateDirectory(string path);
}

internal class DirectoryHandler
    : IDirectoryHandler
{
    public bool Exists(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }

    public DirectoryInfo CreateDirectory(string path)
    {
        return Directory.CreateDirectory(path);
    }
}
