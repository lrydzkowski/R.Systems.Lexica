using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Api.Web;
using R.Systems.Lexica.Core.App.Queries.GetAppInfo;
using RestSharp;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.ExceptionMiddleware;

internal static class RestClientBuilder
{
    public static RestClient BuildWithCustomGetAppInfoHandler(this WebApplicationFactory<Program> webApplicationFactory)
    {
        HttpClient httpClient = webApplicationFactory.WithWebHostBuilder(
                builder =>
                {
                    builder.ConfigureServices(
                        services =>
                        {
                            services
                                .AddTransient<IRequestHandler<GetAppInfoQuery, GetAppInfoResult>,
                                    GetAppInfoHandlerWithException>();
                        }
                    );
                }
            )
            .CreateClient();

        return new RestClient(httpClient);
    }
}
