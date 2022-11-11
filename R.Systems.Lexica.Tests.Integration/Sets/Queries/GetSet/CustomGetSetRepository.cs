using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;

namespace R.Systems.Lexica.Tests.Integration.Sets.Queries.GetSet;

internal class CustomGetSetRepository : IGetSetRepository
{
    public static Set Set => new()
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
    };

    public Task<List<Entry>> GetSetEntriesAsync(string filePath)
    {
        return Task.FromResult(Set.Entries);
    }
}
