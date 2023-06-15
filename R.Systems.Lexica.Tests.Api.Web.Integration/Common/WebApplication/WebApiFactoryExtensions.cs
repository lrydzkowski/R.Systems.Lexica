using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using R.Systems.Lexica.Api.Web;
using R.Systems.Lexica.Infrastructure.Storage.Services;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Authentication;
using RestSharp;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;

internal static class WebApiFactoryExtensions
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

    public static WebApplicationFactory<Program> MockDirectoryExists(
        this WebApplicationFactory<Program> webApplicationFactory,
        bool directoryExists = true
    )
    {
        Mock<IDirectoryHandler> mock = new();
        mock.Setup(x => x.Exists(It.IsAny<string>())).Returns(directoryExists);

        return webApplicationFactory.ReplaceService<IDirectoryHandler>(mock.Object, ServiceLifetime.Singleton);
    }

    public static WebApplicationFactory<Program> ReplaceService<T>(
        this WebApplicationFactory<Program> webApplicationFactory,
        T instance,
        ServiceLifetime serviceLifetime
    ) where T : class
    {
        return webApplicationFactory.ReplaceService<T>(
            new List<T>
            {
                instance
            },
            serviceLifetime
        );
    }

    public static WebApplicationFactory<Program> ReplaceService<T>(
        this WebApplicationFactory<Program> webApplicationFactory,
        IReadOnlyCollection<T> instances,
        ServiceLifetime serviceLifetime
    ) where T : class
    {
        return webApplicationFactory.WithWebHostBuilder(
            builder => builder.ConfigureServices(
                services =>
                {
                    RemoveService(services, typeof(T));
                    foreach (T instance in instances)
                    {
                        RegisterService(services, instance, serviceLifetime);
                    }
                }
            )
        );
    }

    private static void RemoveService(IServiceCollection services, Type serviceType)
    {
        RemoveService(services, d => d.ServiceType == serviceType);
    }

    private static void RemoveService(IServiceCollection services, Type serviceType, Type implementationType)
    {
        RemoveService(services, d => d.ServiceType == serviceType && d.ImplementationType == implementationType);
    }

    private static void RemoveService(IServiceCollection services, Func<ServiceDescriptor, bool> condition)
    {
        List<ServiceDescriptor> serviceDescriptors = services.Where(condition).ToList();
        foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
        {
            services.Remove(serviceDescriptor);
        }
    }

    private static void RegisterService<T>(
        IServiceCollection services,
        T instance,
        ServiceLifetime serviceLifetime
    ) where T : class
    {
        switch (serviceLifetime)
        {
            case ServiceLifetime.Transient:
                services.AddTransient<T>(_ => instance);
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped<T>(_ => instance);
                break;
            case ServiceLifetime.Singleton:
                services.AddSingleton<T>(_ => instance);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
        }
    }
}
