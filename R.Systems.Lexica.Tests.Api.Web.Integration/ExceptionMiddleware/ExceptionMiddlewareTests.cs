using FluentAssertions;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;
using R.Systems.Lexica.Core.Queries.GetAppInfo;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.ExceptionMiddleware;

[Collection(StandardCollection.CollectionName)]
[Trait(TestConstants.Category, StandardCollection.CollectionName)]
public class ExceptionMiddlewareTests
{
    public ExceptionMiddlewareTests(WebApiFactory webApiFactory)
    {
        WebApiFactory = webApiFactory;
    }

    private WebApiFactory WebApiFactory { get; }

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
