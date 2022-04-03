using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Core.Common.Settings;
using R.Systems.Lexica.Core.Sets.Commands.CreateSet;

namespace R.Systems.Lexica.Core;

public static class DependencyInjection
{
    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.Configure<LexicaSettings>(configuration.GetSection(LexicaSettings.PropertyName));
        services.AddScoped<SetValidator>();
    }
}
