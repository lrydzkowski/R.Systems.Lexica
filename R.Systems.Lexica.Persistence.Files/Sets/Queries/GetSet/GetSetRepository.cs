using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;

namespace R.Systems.Lexica.Persistence.Files.Sets.Queries.GetSet;

internal class GetSetRepository : IGetSetRepository
{
    public Task<Set> GetSetAsync(string name)
    {
        // TODO: Remove temporary code.
        Set set = new()
        {
            Name = "Test1",
            Entries = new()
            {
                new()
                {
                    Words = new() { "word1" },
                    Translations = new() { "translation1" }
                }
            }
        };

        return Task.FromResult(set);
    }
}
