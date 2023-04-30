using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Core.Commands.CreateSet;
using R.Systems.Lexica.Core.Commands.DeleteSet;
using R.Systems.Lexica.Core.Commands.UpdateSet;
using R.Systems.Lexica.Core.Queries.GetSet;
using R.Systems.Lexica.Core.Queries.GetSets;
using R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Options;
using R.Systems.Lexica.Infrastructure.Db.SqlServer.Repositories;

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureDbSqlServerServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.ConfigureOptions(configuration);
        services.ConfigureAppDbContext();
        services.ConfigureServices();
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<ConnectionStringsOptions, ConnectionStringsOptionsValidator>(
            configuration,
            ConnectionStringsOptions.Position
        );
    }

    private static void ConfigureAppDbContext(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(
            (serviceProvider, options) =>
            {
                ConnectionStringsOptions connectionStrings =
                    serviceProvider.GetRequiredService<IOptions<ConnectionStringsOptions>>().Value;
                options.UseSqlServer(connectionStrings.AppDb);
            }
        );
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IGetSetsRepository, SetsRepository>();
        services.AddScoped<IGetSetRepository, SetsRepository>();
        services.AddScoped<IDeleteSetRepository, SetsRepository>();
        services.AddScoped<ICreateSetRepository, SetsRepository>();
        services.AddScoped<IUpdateSetRepository, SetsRepository>();
        services.AddScoped<IWordTypesRepository, WordTypesRepository>();
    }
}
