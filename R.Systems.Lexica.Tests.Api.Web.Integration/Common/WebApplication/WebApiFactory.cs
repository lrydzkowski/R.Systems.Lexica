﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Api.Web;
using R.Systems.Lexica.Infrastructure.Db.Common.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Options.Auth0;
using R.Systems.Lexica.Tests.Api.Web.Integration.Options.ConnectionStrings;
using R.Systems.Lexica.Tests.Api.Web.Integration.Options.EnglishDictionary;
using R.Systems.Lexica.Tests.Api.Web.Integration.Options.HealthCheck;
using R.Systems.Lexica.Tests.Api.Web.Integration.Options.Storage;
using R.Systems.Lexica.Tests.Api.Web.Integration.Options.Wordnik;
using RunMethodsSequentially;
using Testcontainers.PostgreSql;
using WireMock.Server;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;

public class WebApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithCleanUp(true)
        .Build();

    private readonly List<IOptionsData> _defaultOptionsData = new()
    {
        new Auth0OptionsData(), new ConnectionStringsOptionsData(), new EnglishDictionaryOptionsData(),
        new HealthCheckOptionsData(), new StorageOptionsData(), new WordnikOptionsData()
    };

    public WebApiFactory()
    {
        WireMockServer = WireMockServer.Start();
    }

    public WireMockServer WireMockServer { get; }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        WireMockServer.Dispose();
        await _dbContainer.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(
            (_, configBuilder) =>
            {
                SetDefaultOptions(configBuilder);
                SetDatabaseConnectionString(configBuilder);
                DisableLogging(configBuilder);
            }
        );
    }

    private void SetDefaultOptions(IConfigurationBuilder configBuilder)
    {
        foreach (IOptionsData optionsData in _defaultOptionsData)
        {
            configBuilder.AddInMemoryCollection(optionsData.ConvertToInMemoryCollection());
        }
    }

    private void SetDatabaseConnectionString(IConfigurationBuilder configBuilder)
    {
        configBuilder.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                [$"{ConnectionStringsOptions.Position}:{nameof(ConnectionStringsOptions.AppPostgresDb)}"] =
                    BuildConnectionString()
            }
        );
    }

    private string BuildConnectionString()
    {
        return _dbContainer.GetConnectionString();
    }

    private void DisableLogging(IConfigurationBuilder configBuilder)
    {
        configBuilder.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["Serilog:MinimumLevel:Default"] = "6",
                ["Serilog:MinimumLevel:Override:Microsoft"] = "6",
                ["Serilog:MinimumLevel:Override:System"] = "6"
            }
        );
    }
}

public class WebApiFactoryWithDb<TDbInitializer> : WebApiFactory
    where TDbInitializer : class, IStartupServiceToRunSequentially
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(ConfigureDbInitializer);
    }

    private void ConfigureDbInitializer(IServiceCollection services)
    {
        services.RegisterRunMethodsSequentially(
                options => options.AddFileSystemLockAndRunMethods(Environment.CurrentDirectory)
            )
            .RegisterServiceToRunInJob<TDbInitializer>();
    }
}
