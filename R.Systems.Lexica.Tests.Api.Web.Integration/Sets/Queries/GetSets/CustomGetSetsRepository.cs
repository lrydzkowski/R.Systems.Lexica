using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Sets.Queries.GetSets;

internal class CustomGetSetsRepository : IGetSetsRepository
{
    public static ListInfo<Set> Sets => new()
    {
        Data = new List<Set>
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
        },
        Count = 1
    };

    public Task<ListInfo<Set>> GetSetsAsync(ListParameters listParameters, bool _)
    {
        return Task.FromResult(Sets);
    }
}
