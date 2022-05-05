using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Lexica.FunctionalTests.Common.Initializers;
using R.Systems.Lexica.FunctionalTests.Common.Settings;
using R.Systems.Lexica.FunctionalTests.Common.Tests;
using R.Systems.Lexica.FunctionalTests.Sets.Queries.GetSets.Parameters;
using R.Systems.Lexica.WebApi;
using R.Systems.Shared.Core.Validation;
using Xunit;

namespace R.Systems.Lexica.FunctionalTests.Sets.Queries.GetSets;

public class GetSetsTests : TestsBase
{
    [Fact]
    public async Task GetSets_WithoutAuthenticationToken_Unauthorized()
    {
        await SendRequestWithoutAuthenticationTokenAsync<List<Set>>(SetsUrl);
    }

    [Fact]
    public async Task GetSets_UserWithoutRoleLexica_Forbidden()
    {
        HttpClient httpClient = new TestWebApplicationFactory<Program>(AssetsPaths.SetCorrectFilesDirPath)
            .CreateClient();
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "lexica@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: EmbeddedRsaKeys.PrivateKey ?? ""
        );

        (HttpStatusCode httpStatusCode, List<Set>? sets) = await RequestService.SendGetAsync<List<Set>>(
            SetsUrl,
            httpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.Forbidden, httpStatusCode);
        Assert.Null(sets);
    }

    [Fact]
    public async Task GetSets_PathToDirWithCorrectSets_ReturnsSets()
    {
        HttpClient httpClient = new TestWebApplicationFactory<Program>(AssetsPaths.SetCorrectFilesDirPath)
            .CreateClient();
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "lexica@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "lexica" },
            privateKeyPem: EmbeddedRsaKeys.PrivateKey ?? ""
        );

        (HttpStatusCode httpStatusCode, List<Set>? sets) = await RequestService.SendGetAsync<List<Set>>(
            SetsUrl,
            httpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.OK, httpStatusCode);
        Assert.NotNull(sets);
        Assert.Equal(5, sets?.Count);
    }

    [Theory]
    [MemberData(nameof(GetSets_PathToDirWithIncorrectSets.Get), MemberType = typeof(GetSets_PathToDirWithIncorrectSets))]
    public async Task GetSets_PathToDirWithIncorrectSets_ReturnsErrorsList(
        string setFilesDirPath, List<ErrorInfo> expectedErrors)
    {
        HttpClient httpClient = new TestWebApplicationFactory<Program>(setFilesDirPath).CreateClient();
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "lexica@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "lexica" },
            privateKeyPem: EmbeddedRsaKeys.PrivateKey ?? ""
        );

        (HttpStatusCode httpStatusCode, List<ErrorInfo>? errors) = await RequestService.SendGetAsync<List<ErrorInfo>>(
            SetsUrl,
            httpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.BadRequest, httpStatusCode);
        Assert.NotNull(errors);
        errors?.Should().BeEquivalentTo(expectedErrors);
    }
}
