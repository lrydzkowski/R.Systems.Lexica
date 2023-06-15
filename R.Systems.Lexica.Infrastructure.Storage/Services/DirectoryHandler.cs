namespace R.Systems.Lexica.Infrastructure.Storage.Services;

internal interface IDirectoryHandler
{
    bool Exists(string directoryPath);
}

internal class DirectoryHandler
    : IDirectoryHandler
{
    public bool Exists(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }
}
