using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Core.Common.Settings;

namespace R.Systems.Lexica.Core;

public static class DependencyInjection
{
    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LexicaSettings>(configuration.GetSection(LexicaSettings.PropertyName));
    }
}
