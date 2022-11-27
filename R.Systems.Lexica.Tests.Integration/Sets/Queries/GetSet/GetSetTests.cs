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
        RestClient restClient = new WebApiFactory<Program>()
            .WithoutAuthentication()
            .WithScopedService<IGetSetRepository, CustomGetSetRepository>()
            .CreateRestClient();
        RestRequest restRequest = new($"{_endpointUrlPath}/{string.Join(",", setPaths)}");

        RestResponse<List<Set>> response = await restClient.ExecuteAsync<List<Set>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedResponse);
    }
}
