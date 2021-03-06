using R.Systems.Lexica.Core;
using R.Systems.Lexica.Persistence.Files;
using R.Systems.Shared.Core.DependencyInjection;
using R.Systems.Shared.WebApi.DependencyInjection;

namespace R.Systems.Lexica.WebApi;

public static class DependencyInjection
{
    public static void AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutomaticServices();
        services.AddCoreServices(configuration);
        services.AddPersistenceFilesServices();
        services.AddJwtSettingsServices(configuration);
        services.AddJwtServices();
        services.AddSwaggerServices(swaggerPageTitle: "R.Systems.Lexica.WebApi");
    }
}
