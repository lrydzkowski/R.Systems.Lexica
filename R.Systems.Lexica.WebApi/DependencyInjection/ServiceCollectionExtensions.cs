using R.Systems.Lexica.Infrastructure.DependencyInjection;
using R.Systems.Shared.Core.DependencyInjection;
using R.Systems.Shared.WebApi.DependencyInjection;

namespace R.Systems.Lexica.WebApi.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutomaticServices();
        services.AddInfrastructureServices(configuration);
        services.AddJwtSettingsServices(configuration);
        services.AddJwtServices();
        services.AddSwaggerServices(swaggerPageTitle: "R.Systems.Lexica.WebApi");
    }
}
