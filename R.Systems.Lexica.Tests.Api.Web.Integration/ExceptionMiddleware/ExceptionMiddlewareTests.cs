using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using R.Systems.Lexica.Core.Queries.GetAppInfo;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;
using R.Systems.Lexica.Api.Web;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.ExceptionMiddleware;

[Collection(StandardCollection.CollectionName)]
[Trait(TestConstants.Category, StandardCollection.CollectionName)]
public class ExceptionMiddlewareTests
{
    public ExceptionMiddlewareTests(WebApiFactory webApiFactory)
    {
        WebApiFactory = webApiFactory.MockDirectoryExists();
    }

    private WebApplicationFactory<Program> WebApiFactory { get; }

    [Fact]
    public async Task GetAppInfo_ShouldReturn500InternalServerError_WhenUnexpectedExceptionWasThrown()
    {
        RestClient restClient = WebApiFactory.BuildWithCustomGetAppInfoHandler();

        RestRequest request = new("/");

        RestResponse response = await restClient.ExecuteAsync<GetAppInfoResult>(request);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Content.Should().Be("");
    }
}
