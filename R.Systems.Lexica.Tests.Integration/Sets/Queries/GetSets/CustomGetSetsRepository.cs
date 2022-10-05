using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;

namespace R.Systems.Lexica.Tests.Integration.Sets.Queries.GetSets;

internal class CustomGetSetsRepository : IGetSetsRepository
{
    public static List<Set> Sets => new()
    {
        new()
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
        }
    };

    public Task<List<Set>> GetSetsAsync(ListParameters listParameters, bool _)
    {
        return Task.FromResult(Sets);
    }
}
