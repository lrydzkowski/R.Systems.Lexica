using FluentAssertions;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Api;

[Collection(StandardCollection.CollectionName)]
[Trait(TestConstants.Category, StandardCollection.CollectionName)]
public class ApiTests
{
    public ApiTests(WebApiFactory webApiFactory)
    {
        RestClient = webApiFactory.CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Theory(Skip = "No data ;)")]
    [MemberData(nameof(ApiDataBuilder.Build), MemberType = typeof(ApiDataBuilder))]
    public async Task SendRequest_ShouldReturn401_WhenNoAccessToken(string endpointUrlPath, Method httpMethod)
    {
        RestRequest restRequest = new(endpointUrlPath, httpMethod);

        RestResponse response = await RestClient.ExecuteAsync(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
