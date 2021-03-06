namespace R.Systems.Lexica.Persistence.Files.Common;

internal class SetFileSource : ISetSource
{
    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    public bool DirExists(string dirPath)
    {
        return Directory.Exists(dirPath);
    }

    public async Task<string> GetContentAsync(string path)
    {
        return await File.ReadAllTextAsync(path);
    }

    public List<string> GetSetNames(string dirPath)
    {
        DirectoryInfo dirInfo = new(dirPath);
        return dirInfo.GetFiles("*.*").Select(file => file.Name).ToList();
    }

    public async Task CreateSetAsync(string path, string content)
    {
        await File.WriteAllTextAsync(path, content);
    }
}
