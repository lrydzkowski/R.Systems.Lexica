using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.FunctionalTests.Common.Services;
using R.Systems.Lexica.Persistence.Files.Common;
using R.Systems.Shared.Core.Interfaces;

namespace R.Systems.Lexica.FunctionalTests.Common.Initializers;

public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public TestWebApplicationFactory(string setFilesDirPath)
    {
        SetFilesDirPath = setFilesDirPath;
    }

    public string SetFilesDirPath { get; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        OverrideConfiguration(builder);
        builder.ConfigureServices(services =>
        {
            ReplaceIRsaKeysLoader(services);
            ReplaceSetSource(services);
        });
    }

    private void OverrideConfiguration(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    ["Lexica:SetFilesDirPath"] = SetFilesDirPath,
                    ["Jwt:PublicKeyPemFilePath"] = "public.pem"
                }
            );
        });
    }

    private void ReplaceIRsaKeysLoader(IServiceCollection services)
    {
        RemoveService(services, typeof(IRsaKeys));
        services.AddSingleton<IRsaKeys, EmbeddedRsaKeys>();
    }

    private void ReplaceSetSource(IServiceCollection services)
    {
        RemoveService(services, typeof(ISetSource));
        services.AddSingleton<ISetSource, SetEmbeddedFileSource>();
    }

    private void RemoveService(IServiceCollection services, Type serviceType)
    {
        ServiceDescriptor? serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);
        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }
    }
}
