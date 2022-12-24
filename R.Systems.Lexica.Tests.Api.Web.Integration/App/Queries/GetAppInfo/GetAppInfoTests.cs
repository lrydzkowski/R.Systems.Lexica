using FluentAssertions;
using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.App.Queries.GetAppInfo;

[Collection(MainTestsCollection.CollectionName)]
[Trait(TestConstants.Category, MainTestsCollection.CollectionName)]
public class GetAppInfoTests : IClassFixture<WebApiFactory>
{
    public GetAppInfoTests(WebApiFactory webApiFactory)
    {
        RestClient = webApiFactory.CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetAppInfo_ShouldReturnCorrectVersion_WhenCorrectDataIsPassed()
    {
        string expectedAppName = AppNameService.GetWebApiName();
        string semVerRegex = new SemVerRegex().Get();
        RestRequest request = new("/");

        RestResponse<GetAppInfoResponse> response = await RestClient.ExecuteAsync<GetAppInfoResponse>(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data?.AppName.Should().Be(expectedAppName);
        response.Data?.AppVersion.Should().MatchRegex(semVerRegex);
    }
}
