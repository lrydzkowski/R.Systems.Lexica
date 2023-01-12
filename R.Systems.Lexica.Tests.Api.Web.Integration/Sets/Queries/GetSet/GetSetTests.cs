using FluentAssertions;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Sets.Queries.GetSet;

[Collection(MainTestsCollection.CollectionName)]
[Trait(TestConstants.Category, MainTestsCollection.CollectionName)]
public class GetSetTests : IClassFixture<WebApiFactory>
{
    private readonly string _endpointUrlPath = "/sets/content";

    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new List<string> { "Test11" } },
            new object[] { new List<string> { "Test11", "Test22" } }
        };

    [Theory]
    [MemberData(nameof(Data))]
    public async Task GetSet_ShouldReturnSet_WhenCorrectData(List<string> setPaths)
    {
        List<Set> expectedResponse = setPaths.Select(setPath => CustomGetSetRepository.Sets[setPath]).ToList();
        RestClient restClient = new WebApiFactory()
            .WithoutAuthentication()
            .WithScopedService<IGetSetRepository, CustomGetSetRepository>()
            .CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);
        foreach (string setPath in setPaths)
        {
            restRequest.AddQueryParameter(nameof(setPath), setPath);
        }

        RestResponse<List<Set>> response = await restClient.ExecuteAsync<List<Set>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedResponse);
    }
}
