using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;

namespace R.Systems.Lexica.Tests.Integration.Sets.Queries.GetSet;

internal class CustomGetSetRepository : IGetSetRepository
{
    public static Dictionary<string, Set> Sets = new()
    {
        ["Test11"] = new()
        {
            Path = "Test11",
            Entries = new()
            {
                new()
                {
                    Words = new() { "word11" },
                    Translations = new() { "translation11" }
                }
            }
        },
        ["Test22"] = new()
        {
            Path = "Test22",
            Entries = new()
            {
                new()
                {
                    Words = new() { "word22" },
                    Translations = new() { "translation22" }
                }
            }
        }
    };

    public Task<List<Entry>> GetSetEntriesAsync(string filePath)
    {
        return Task.FromResult(!Sets.ContainsKey(filePath) ? new List<Entry>() : Sets[filePath].Entries);
    }
}
