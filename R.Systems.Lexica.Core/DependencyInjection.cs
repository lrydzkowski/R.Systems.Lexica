using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly.Caching;
using Polly.Caching.Memory;
using R.Systems.Lexica.Core.Common.Api;
using R.Systems.Lexica.Core.Common.Validation;

namespace R.Systems.Lexica.Core;

public static class DependencyInjection
{
    public static void ConfigureCoreServices(this IServiceCollection services)
    {
        services.AddMediatR();
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.ConfigureServices();
    }

    public static void ConfigureOptionsWithValidation<TOptions, TValidator>(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationPosition
    )
        where TOptions : class
        where TValidator : class, IValidator<TOptions>, new()
    {
        services.AddSingleton<IValidator<TOptions>, TValidator>();
        services.AddOptions<TOptions>()
            .Bind(configuration.GetSection(configurationPosition))
            .ValidateFluently()
            .ValidateOnStart();
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
