namespace R.Systems.Lexica.Persistence.Files.Common;

public interface ISetSource
{
    public bool Exists(string path);

    public bool DirExists(string dirPath);

    public Task<string> GetContentAsync(string path);

    public List<string> GetSetNames(string dirPath);

    public Task CreateSetAsync(string path, string content);
}
