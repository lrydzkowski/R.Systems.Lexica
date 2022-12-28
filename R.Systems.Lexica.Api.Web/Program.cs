using NLog;
using NLog.Web;
using R.Systems.Lexica.Api.Web.Middleware;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Infrastructure.Azure;
using R.Systems.Lexica.Infrastructure.Forvo;

namespace R.Systems.Lexica.Api.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("init main");

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
            logger.Error(exception, "Stopped program because of exception");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.ConfigureServices(builder.Environment);
        builder.Services.ConfigureCoreServices();
        builder.Services.ConfigureInfrastructureAzureServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureForvoServices(builder.Configuration);
    }

    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
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
