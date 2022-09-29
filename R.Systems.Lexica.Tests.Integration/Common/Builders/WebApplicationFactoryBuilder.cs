using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Tests.Integration.Common.Authentication;
using R.Systems.Lexica.WebApi;
using RestSharp;

namespace R.Systems.Lexica.Tests.Integration.Common.Builders;

internal static class WebApplicationFactoryBuilder
{
    public static RestClient CreateRestClient(this WebApplicationFactory<Program> webApplicationFactory)
    {
        return new RestClient(webApplicationFactory.CreateClient());
    }

    public static WebApplicationFactory<Program> WithoutAuthentication(
        this WebApplicationFactory<Program> webApplicationFactory
    )
    {
        return webApplicationFactory.WithWebHostBuilder(
            builder => builder.ConfigureServices(
                services => services.AddSingleton<IAuthorizationHandler, AllowAnonymous>()
            )
        );
    }

    public static WebApplicationFactory<Program> WithCustomOptions(
        this WebApplicationFactory<Program> webApplicationFactory,
        Dictionary<string, string?> customOptions
    )
    {
        return webApplicationFactory.WithWebHostBuilder(
            builder => builder.ConfigureAppConfiguration(
                (_, configBuilder) => configBuilder.AddInMemoryCollection(customOptions)
            )
        );
    }

    public static WebApplicationFactory<Program> WithScopedService<TService, TImplementation>(
        this WebApplicationFactory<Program> webApplicationFactory
    ) where TService : class where TImplementation : class, TService
    {
        return webApplicationFactory.WithWebHostBuilder(
            builder => builder.ConfigureServices(
                services => services.AddScoped<TService, TImplementation>()
            )
        );
    }
}
