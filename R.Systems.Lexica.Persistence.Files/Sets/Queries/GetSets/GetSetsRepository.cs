using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;

namespace R.Systems.Lexica.Persistence.Files.Sets.Queries.GetSets;

internal class GetSetsRepository : IGetSetsRepository
{
    public Task<List<Set>> GetSetsAsync(ListParameters listParameters)
    {
        // TODO: Remove temporary code.
        List<Set> sets = new()
        {
            new()
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
            }
        };

        return Task.FromResult(sets);
    }
}
