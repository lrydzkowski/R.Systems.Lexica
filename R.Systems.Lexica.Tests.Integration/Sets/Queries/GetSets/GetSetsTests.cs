using FluentAssertions;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using R.Systems.Lexica.Tests.Integration.Common.Builders;
using R.Systems.Lexica.Tests.Integration.Common.Factories;
using R.Systems.Lexica.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Lexica.Tests.Integration.Sets.Queries.GetSets;

public class GetSetsTests
{
    private readonly string _endpointUrlPath = "/sets";

    [Fact]
    public async Task GetSet_ShouldReturnSet()
    {
        ListInfo<Set> expectedResponse = CustomGetSetsRepository.Sets;
        RestClient restClient = new WebApiFactory<Program>()
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
