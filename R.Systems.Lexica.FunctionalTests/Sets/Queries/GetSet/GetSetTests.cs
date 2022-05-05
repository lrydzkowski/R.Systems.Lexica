using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Lexica.FunctionalTests.Common.Initializers;
using R.Systems.Lexica.FunctionalTests.Common.Settings;
using R.Systems.Lexica.FunctionalTests.Common.Tests;
using R.Systems.Lexica.FunctionalTests.Sets.Queries.GetSet.Parameters;
using R.Systems.Lexica.WebApi;
using R.Systems.Shared.Core.Validation;
using Xunit;

namespace R.Systems.Lexica.FunctionalTests.Sets.Queries.GetSet;

public class GetSetTests : TestsBase
{
    [Fact]
    public async Task GetSet_WithoutAuthenticationToken_Unauthorized()
    {
        const string setFileName = "example_set_1.txt";
        string setUrl = $"{SetsUrl}/{setFileName}";
        await SendRequestWithoutAuthenticationTokenAsync<Set>(setUrl);
    }

    [Fact]
    public async Task GetSet_UserWithoutRoleLexica_Forbidden()
    {
        const string setFileName = "example_set_1.txt";
        string setUrl = $"{SetsUrl}/{setFileName}";
        HttpClient httpClient = new TestWebApplicationFactory<Program>(AssetsPaths.SetCorrectFilesDirPath)
            .CreateClient();
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "lexica@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: EmbeddedRsaKeys.PrivateKey ?? ""
        );

        (HttpStatusCode httpStatusCode, Set? set) = await RequestService.SendGetAsync<Set>(
            setUrl,
            httpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.Forbidden, httpStatusCode);
        Assert.Null(set);
    }

    [Theory]
    [MemberData(nameof(GetSet_PathToCorrectSet.Get), MemberType = typeof(GetSet_PathToCorrectSet))]
    public async Task GetSet_PathToCorrectSet_ReturnsSet(
        string setFilesDirPath, string setFileName, int numOfEntries, Entry firstEntry, Entry lastEntry)
    {
        string setUrl = $"{SetsUrl}/{setFileName}";
        HttpClient httpClient = new TestWebApplicationFactory<Program>(setFilesDirPath).CreateClient();
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "lexica@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "lexica" },
            privateKeyPem: EmbeddedRsaKeys.PrivateKey ?? ""
        );

        (HttpStatusCode httpStatusCode, Set? set) = await RequestService.SendGetAsync<Set>(
            setUrl,
            httpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.OK, httpStatusCode);
        Assert.NotNull(set);
        Assert.Equal(setFileName, set?.Name);
        Assert.Equal(numOfEntries, set?.Entries.Count);
        set?.Entries[0].Should().BeEquivalentTo(firstEntry);
        set?.Entries.Last().Should().BeEquivalentTo(lastEntry);
    }

    [Theory]
    [MemberData(nameof(GetSet_PathToIncorrectSet.Get), MemberType = typeof(GetSet_PathToIncorrectSet))]
    public async Task GetSet_PathToIncorrectSet_ReturnsErrorsList(
        string setFilesDirPath, string setFileName, List<ErrorInfo> expectedErrors)
    {
        string setUrl = $"{SetsUrl}/{setFileName}";
        HttpClient httpClient = new TestWebApplicationFactory<Program>(setFilesDirPath).CreateClient();
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "lexica@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "lexica" },
            privateKeyPem: EmbeddedRsaKeys.PrivateKey ?? ""
        );

        (HttpStatusCode httpStatusCode, List<ErrorInfo>? errors) = await RequestService.SendGetAsync<List<ErrorInfo>>(
            setUrl,
            httpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.BadRequest, httpStatusCode);
        Assert.NotNull(errors);
        errors?.Should().BeEquivalentTo(expectedErrors);
    }
}
