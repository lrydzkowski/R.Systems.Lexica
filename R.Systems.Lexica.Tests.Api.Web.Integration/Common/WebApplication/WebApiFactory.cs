using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using R.Systems.Lexica.Api.Web;
using R.Systems.Lexica.Tests.Api.Web.Integration.Options.AzureAd;
using R.Systems.Lexica.Tests.Api.Web.Integration.Options.AzureFiles;
using R.Systems.Lexica.Tests.Api.Web.Integration.Options.PronunciationApi;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;

public class WebApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(
            (_, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new AzureAdOptionsData().ConvertToInMemoryCollection());
                configBuilder.AddInMemoryCollection(new AzureFilesOptionsData().ConvertToInMemoryCollection());
                configBuilder.AddInMemoryCollection(new PronunciationApiOptionsData().ConvertToInMemoryCollection());
            }
        );
    }
}
