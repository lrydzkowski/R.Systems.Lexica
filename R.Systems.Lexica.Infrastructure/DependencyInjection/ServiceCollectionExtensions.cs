using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Core.Interfaces;
using R.Systems.Lexica.Infrastructure.Repositories;
using R.Systems.Lexica.Infrastructure.Settings;
using R.Systems.Lexica.Infrastructure.Sources;

namespace R.Systems.Lexica.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<InfrastructureSettings>(configuration.GetSection(InfrastructureSettings.PropertyName));
        services.AddScoped<ISetRepository, SetRepository>();
        services.AddScoped<ISetSource, SetFileSource>();
    }
}
