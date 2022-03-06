using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Core.Common.Interfaces;
using R.Systems.Lexica.Infrastructure.Persistence.Files.Sets;

namespace R.Systems.Lexica.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ISetRepository, SetRepository>();
        services.AddScoped<ISetSource, SetFileSource>();
    }
}
