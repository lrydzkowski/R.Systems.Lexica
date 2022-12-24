using FluentAssertions;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Sets.Queries.GetSets;

[Collection(MainTestsCollection.CollectionName)]
[Trait(TestConstants.Category, MainTestsCollection.CollectionName)]
public class GetSetsTests : IClassFixture<WebApiFactory>
{
    private readonly string _endpointUrlPath = "/sets";

    [Fact]
    public async Task GetSets_ShouldReturnSets_WhenCorrectData()
    {
        ListInfo<Set> expectedResponse = CustomGetSetsRepository.Sets;
        RestClient restClient = new WebApiFactory()
            .WithoutAuthentication()
            .WithScopedService<IGetSetsRepository, CustomGetSetsRepository>()
            .CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<ListInfo<Set>> response = await restClient.ExecuteAsync<ListInfo<Set>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedResponse);
    }
}
