using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Lexica.FunctionalTests.Initializers;
using R.Systems.Lexica.WebApi;
using R.Systems.Shared.Core.Validation;
using Xunit;

namespace R.Systems.Lexica.FunctionalTests.Tests.SetTests;

public class GetSetsTests : SetControllerTests
{
    [Fact]
    public async Task GetSets_WithoutAuthenticationToken_Unauthorized()
    {
        string setFilesDirPath = "_Assets/Sets/Correct/";
        HttpClient httpClient = new CustomWebApplicationFactory<Program>(setFilesDirPath).CreateClient();

        (HttpStatusCode httpStatusCode, List<Set>? sets) = await RequestService.SendGetAsync<List<Set>>(
            SetsUrl,
            httpClient
        );

        Assert.Equal(HttpStatusCode.Unauthorized, httpStatusCode);
        Assert.Null(sets);
    }

    [Fact]
    public async Task GetSets_UserWithoutRoleLexica_Forbidden()
    {
        string setFilesDirPath = "_Assets/Sets/Correct/";
        HttpClient httpClient = new CustomWebApplicationFactory<Program>(setFilesDirPath).CreateClient();
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "lexica@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: EmbeddedRsaKeys.PrivateKey ?? ""
        );

        (HttpStatusCode httpStatusCode, Set? set) = await RequestService.SendGetAsync<Set>(
            SetsUrl,
            httpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.Forbidden, httpStatusCode);
        Assert.Null(set);
    }

    [Fact]
    public async Task GetSets_PathToDirWithCorrectSets_ReturnsSets()
    {
        string setFilesDirPath = "_Assets/Sets/Correct/";
        HttpClient httpClient = new CustomWebApplicationFactory<Program>(setFilesDirPath).CreateClient();
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
    [MemberData(nameof(GetParametersFor_GetSets_PathToDirWithIncorrectSets))]
    public async Task GetSets_PathToDirWithIncorrectSets_ReturnsErrorsList(
        string setFilesDirPath, List<ErrorInfo> expectedErrors)
    {
        HttpClient httpClient = new CustomWebApplicationFactory<Program>(setFilesDirPath).CreateClient();
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

    public static IEnumerable<object[]> GetParametersFor_GetSets_PathToDirWithIncorrectSets()
    {
        string setFilesDirPath = @"_Assets\Sets";
        return new List<object[]>
        {
            new object[]
            {
                Path.Combine(setFilesDirPath, "NoSemicolon"),
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "NoSemicolon",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "17" }
                    ),
                    new ErrorInfo(
                        errorKey: "NoSemicolon",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "21" }
                    ),
                    new ErrorInfo(
                        errorKey: "NoSemicolon",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "2" }
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "ErrorsCombination"),
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "NoSemicolon",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "12" }
                    ),
                    new ErrorInfo(
                        errorKey: "NoSemicolon",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "17" }
                    ),
                    new ErrorInfo(
                        errorKey: "NoTranslations",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "21" }
                    ),
                    new ErrorInfo(
                        errorKey: "NoWords",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "25" }
                    ),
                    new ErrorInfo(
                        errorKey: "NoSemicolon",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "19" }
                    ),
                    new ErrorInfo(
                        errorKey: "TooManySemicolons",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "23", ["NumOfSemicolons"] = "2" }
                    ),
                    new ErrorInfo(
                        errorKey: "TooLongTranslation",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "25" }
                    ),
                    new ErrorInfo(
                        errorKey: "TooManySemicolons",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "8", ["NumOfSemicolons"] = "2" }
                    ),
                    new ErrorInfo(
                        errorKey: "TooLongWord",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "13" }
                    ),
                    new ErrorInfo(
                        errorKey: "TooLongTranslation",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "18" }
                    )
                }
            }
        };
    }
}
