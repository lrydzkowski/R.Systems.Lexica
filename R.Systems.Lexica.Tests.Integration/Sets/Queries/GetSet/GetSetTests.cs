using FluentAssertions;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Tests.Integration.Common.Builders;
using R.Systems.Lexica.Tests.Integration.Common.Factories;
using R.Systems.Lexica.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Lexica.Tests.Integration.Sets.Queries.GetSet;

public class GetSetTests
{
    private readonly string _endpointUrlPath = "/sets";

    [Fact]
    public async Task GetSet_ShouldReturnSet_WhenCorrectData()
    {
        Set expectedResponse = CustomGetSetRepository.Set;
        RestClient restClient = new WebApiFactory<Program>()
            .WithoutAuthentication()
            .WithScopedService<IGetSetRepository, CustomGetSetRepository>()
            .CreateRestClient();
        RestRequest restRequest = new($"{_endpointUrlPath}/Test11");

        RestResponse<Set> response = await restClient.ExecuteAsync<Set>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedResponse);
    }
}
