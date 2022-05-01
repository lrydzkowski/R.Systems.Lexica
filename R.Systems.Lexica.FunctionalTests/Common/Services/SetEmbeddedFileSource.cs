using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using R.Systems.Lexica.Persistence.Files.Common;

namespace R.Systems.Lexica.FunctionalTests.Common.Services;

internal class SetEmbeddedFileSource : ISetSource
{
    private Dictionary<string, string> CreatedSets { get; } = new();

    public bool Exists(string path)
    {
        path = TransformPath(path);
        Assembly assembly = GetType().Assembly;
        return assembly.GetManifestResourceNames().Any(x => x == path);
    }

    public bool DirExists(string dirPath)
    {
        dirPath = TransformPath(dirPath);
        Assembly assembly = GetType().Assembly;
        return assembly.GetManifestResourceNames().Any(x => x.StartsWith(dirPath));
    }

    public Task<string> GetContentAsync(string path)
    {
        string content = GetContent(path) ?? "";
        return Task.FromResult(content);
    }

    public List<string> GetSetNames(string dirPath)
    {
        if (!DirExists(dirPath))
        {
            return new List<string>();
        }
        dirPath = TransformPath(dirPath);
        Assembly assembly = GetType().Assembly;
        return assembly.GetManifestResourceNames()
            .Where(x => x.StartsWith(dirPath))
            .Select(x => string.Join('.', x.Split('.').Reverse().Take(2).Reverse()))
            .ToList();
    }

    public Task CreateSetAsync(string path, string content)
    {
        CreatedSets[path] = content;
        return Task.CompletedTask;
    }

    private string? GetContent(string path)
    {
        if (!Exists(path))
        {
            return null;
        }
        path = TransformPath(path);
        Assembly assembly = GetType().Assembly;
        using Stream? stream = assembly.GetManifestResourceStream(path);
        if (stream == null)
        {
            return null;
        }
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }

    private string TransformPath(string path)
    {
        string? transformedPath = path.Replace('/', '.').Replace('\\', '.');
        return $"R.Systems.Lexica.FunctionalTests.{transformedPath}";
    }
}
