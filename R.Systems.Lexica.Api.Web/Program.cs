using R.Systems.Lexica.Api.Web.Middleware;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Infrastructure.Azure;
using R.Systems.Lexica.Infrastructure.Db.SqlServer;
using R.Systems.Lexica.Infrastructure.EnglishDictionary;
using R.Systems.Lexica.Infrastructure.Wordnik;
using Serilog;

namespace R.Systems.Lexica.Api.Web;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = Serilog.CreateBootstrapLogger();

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
        builder.Services.ConfigureServices(builder.Environment);
        builder.Services.ConfigureCoreServices();
        builder.Services.ConfigureInfrastructureDbSqlServerServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureAzureServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureEnglishDictionaryServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureWordnikServices(builder.Configuration);
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

        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}
