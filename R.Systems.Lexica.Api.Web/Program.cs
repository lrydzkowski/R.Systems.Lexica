using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using R.Systems.Lexica.Api.Web.Auth;
using R.Systems.Lexica.Api.Web.Middleware;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Infrastructure.Auth0;
using R.Systems.Lexica.Infrastructure.Db;
using R.Systems.Lexica.Infrastructure.EnglishDictionary;
using R.Systems.Lexica.Infrastructure.Storage;
using R.Systems.Lexica.Infrastructure.Wordnik;
using Serilog;
using Serilog.Debugging;

namespace R.Systems.Lexica.Api.Web;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = Serilog.CreateBootstrapLogger();
        SelfLog.Enable(Console.Error);

        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            ConfigureLogging(builder);
            WebApplication app = builder.Build();
            ConfigureRequestPipeline(app);
            app.Run();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Application terminated unexpectedly");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.ConfigureServices(builder.Environment, builder.Configuration);
        builder.Services.ConfigureCoreServices();
        builder.Services.ConfigureInfrastructureDbServices(builder.Configuration);
        //builder.Services.ConfigureInfrastructureAzureServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureAuth0Services(builder.Configuration);
        builder.Services.ConfigureInfrastructureEnglishDictionaryServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureWordnikServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureStorageServices(builder.Configuration);
    }

    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);
        builder.Host.UseSerilog(Serilog.CreateLogger, true);
    }

    private static void ConfigureRequestPipeline(WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSwagger();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI();
        }

        UseHealthChecks(app);
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }

    private static void UseHealthChecks(WebApplication app)
    {
        app.MapHealthChecks(
                "/health",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                }
            )
            .RequireAuthorization(
                builder => builder.AddAuthenticationSchemes(ApiKeyAuthenticationSchemeOptions.Name)
                    .RequireAuthenticatedUser()
            );
    }
}
