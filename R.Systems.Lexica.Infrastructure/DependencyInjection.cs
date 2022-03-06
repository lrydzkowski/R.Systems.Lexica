using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Core.Interfaces;
using R.Systems.Lexica.Infrastructure.Interfaces;
using R.Systems.Lexica.Infrastructure.Repositories;
using R.Systems.Lexica.Infrastructure.Settings;
using R.Systems.Lexica.Infrastructure.Sources;

namespace R.Systems.Lexica.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<InfrastructureSettings>(configuration.GetSection(InfrastructureSettings.PropertyName));
        services.AddScoped<ISetRepository, SetRepository>();
        services.AddScoped<ISetSource, SetFileSource>();
    }
}
