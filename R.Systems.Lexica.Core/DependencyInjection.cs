using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly.Caching;
using Polly.Caching.Memory;
using R.Systems.Lexica.Core.Common.Api;
using R.Systems.Lexica.Core.Common.Validation;

namespace R.Systems.Lexica.Core;

public static class DependencyInjection
{
    private const string SwaggerCliEnvironmentName = "SwaggerCli";

    public static void ConfigureCoreServices(this IServiceCollection services)
    {
        services.AddMediatR();
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.ConfigureServices();
    }

    public static void ConfigureOptionsWithValidation<TOptions, TValidator>(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationPosition,
        IWebHostEnvironment environment
    )
        where TOptions : class
        where TValidator : class, IValidator<TOptions>
    {
        services.AddSingleton<IValidator<TOptions>, TValidator>();
        OptionsBuilder<TOptions> optionsBuilder = services.AddOptions<TOptions>()
            .Bind(configuration.GetSection(configurationPosition))
            .ValidateFluently();
        if (environment.EnvironmentName != SwaggerCliEnvironmentName)
        {
            optionsBuilder.ValidateOnStart();
        }
    }

    private static void AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IApiRetryPolicies<>), typeof(ApiRetryPolicies<>))
            .AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();
    }
}
