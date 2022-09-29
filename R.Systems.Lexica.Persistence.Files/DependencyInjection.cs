using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using R.Systems.Lexica.Persistence.Files.Sets.Queries.GetSet;
using R.Systems.Lexica.Persistence.Files.Sets.Queries.GetSets;

namespace R.Systems.Lexica.Persistence.Files;

public static class DependencyInjection
{
    public static void AddPersistenceFilesServices(this IServiceCollection services)
    {
        services.AddScoped<IGetSetRepository, GetSetRepository>();
        services.AddScoped<IGetSetsRepository, GetSetsRepository>();
    }
}
