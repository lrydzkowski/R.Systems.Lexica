using NLog;
using NLog.Web;
using R.Systems.Lexica.WebApi.Filters;
using R.Systems.Shared.WebApi.Middlewares;

namespace R.Systems.Lexica.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("Starting up!");
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            InitNLog(builder);
            ConfigureServices(builder);
            WebApplication app = builder.Build();
            Configure(app);
            app.Run();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Stopped program because of exception");
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    private static void InitNLog(WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddWebApiServices(builder.Configuration);
    }

    private static void Configure(WebApplication app)
    {
        app.UseGlobalExceptionHandler();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}
