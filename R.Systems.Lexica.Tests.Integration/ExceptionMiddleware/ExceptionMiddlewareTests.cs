using FluentAssertions;
using R.Systems.Lexica.Core.App.Queries.GetAppInfo;
using R.Systems.Lexica.Tests.Integration.Common.Factories;
using R.Systems.Lexica.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Lexica.Tests.Integration.ExceptionMiddleware;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task GetAppInfo_ShouldReturn500InternalServerError_WhenUnexpectedExceptionWasThrown()
    {
        RestClient restClient = new WebApiFactory<Program>().BuildWithCustomGetAppInfoHandler();

        RestRequest request = new("/");

        RestResponse response = await restClient.ExecuteAsync<GetAppInfoResult>(request);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Content.Should().Be("");
    }
}
