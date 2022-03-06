namespace R.Systems.Lexica.Infrastructure.Persistence.Files.Sets;

public interface ISetSource
{
    public bool Exists(string path);

    public bool DirExists(string dirPath);

    public Task<string> GetContentAsync(string path);

    public List<string> GetSetNames(string dirPath);
}
