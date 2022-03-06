using R.Systems.Lexica.Infrastructure.Interfaces;

namespace R.Systems.Lexica.Infrastructure.Sources;

public class SetFileSource : ISetSource
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
        List<string> setsNames = dirInfo.GetFiles("*.*").Select(file => file.Name).ToList();
        return setsNames;
    }
}
