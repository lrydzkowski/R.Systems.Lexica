using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using R.Systems.Lexica.Core.Models;
using R.Systems.Lexica.FunctionalTests.Initializers;
using R.Systems.Lexica.FunctionalTests.Services;
using R.Systems.Lexica.WebApi;
using R.Systems.Shared.Core.Validation;
using Xunit;

namespace R.Systems.Lexica.FunctionalTests.Tests.SetTests;

public class GetSetTests : SetControllerTests
{
    [Fact]
    public async Task GetSet_WithoutAuthenticationToken_Unauthorized()
    {
        string setFilesDirPath = "_Assets/Sets/Correct/";
        string setFileName = "example_set_1.txt";
        HttpClient httpClient = new CustomWebApplicationFactory<Program>(setFilesDirPath).CreateClient();

        (HttpStatusCode httpStatusCode, Set? set) = await RequestService.SendGetAsync<Set>(
            $"{SetsUrl}/{setFileName}",
            httpClient
        );

        Assert.Equal(HttpStatusCode.Unauthorized, httpStatusCode);
        Assert.Null(set);
    }

    [Fact]
    public async Task GetSet_UserWithoutRoleLexica_Forbidden()
    {
        string setFilesDirPath = "_Assets/Sets/Correct/";
        string setFileName = "example_set_1.txt";
        HttpClient httpClient = new CustomWebApplicationFactory<Program>(setFilesDirPath).CreateClient();
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "lexica@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "admin" },
            privateKeyPem: EmbeddedRsaKeys.PrivateKey ?? ""
        );

        (HttpStatusCode httpStatusCode, Set? set) = await RequestService.SendGetAsync<Set>(
            $"{SetsUrl}/{setFileName}",
            httpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.Forbidden, httpStatusCode);
        Assert.Null(set);
    }

    [Theory]
    [MemberData(nameof(GetParametersFor_GetSet_PathToCorrectSet))]
    public async Task GetSet_PathToCorrectSet_ReturnsSet(
        string setFilesDirPath, string setFileName, int numOfEntries, Entry firstEntry, Entry lastEntry)
    {
        HttpClient httpClient = new CustomWebApplicationFactory<Program>(setFilesDirPath).CreateClient();
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "lexica@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "lexica" },
            privateKeyPem: EmbeddedRsaKeys.PrivateKey ?? ""
        );

        (HttpStatusCode httpStatusCode, Set? set) = await RequestService.SendGetAsync<Set>(
            $"{SetsUrl}/{setFileName}",
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

    public static IEnumerable<object[]> GetParametersFor_GetSet_PathToCorrectSet()
    {
        string setFilesDirPath = @"_Assets\Sets\Correct";
        return new List<object[]>
        {
            new object[]
            {
                setFilesDirPath,
                "example_set_1.txt",
                31,
                new Entry
                {
                    Words = new List<string>() { "incomparably" },
                    Translations = new List<string>() { "nieporównywalnie" }
                },
                new Entry
                {
                    Words = new List<string>() { "mist" },
                    Translations = new List<string>() { "mgła" }
                }
            },
            new object[]
            {
                setFilesDirPath,
                "example_set_5.txt",
                28,
                new Entry
                {
                    Words = new List<string>() { "jagged" },
                    Translations = new List<string>() { "postrzępiony", "wyszczerbiony" }
                },
                new Entry
                {
                    Words = new List<string>() { "misfit", "creep" },
                    Translations = new List<string>() { "odmieniec" }
                }
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetParametersFor_GetSet_PathToIncorrectSet))]
    public async Task GetSet_PathToIncorrectSet_ReturnsErrorsList(
        string setFilesDirPath, string setFileName, List<ErrorInfo> expectedErrors)
    {
        HttpClient httpClient = new CustomWebApplicationFactory<Program>(setFilesDirPath).CreateClient();
        string accessToken = AuthenticatorService.GenerateAccessToken(
            userId: 1,
            userEmail: "lexica@lukaszrydzkowski.pl",
            userRolesKeys: new List<string>() { "lexica" },
            privateKeyPem: EmbeddedRsaKeys.PrivateKey ?? ""
        );

        (HttpStatusCode httpStatusCode, List<ErrorInfo>? errors) = await RequestService.SendGetAsync<List<ErrorInfo>>(
            $"{SetsUrl}/{setFileName}",
            httpClient,
            accessToken
        );

        Assert.Equal(HttpStatusCode.BadRequest, httpStatusCode);
        Assert.NotNull(errors);
        errors?.Should().BeEquivalentTo(expectedErrors);
    }

    public static IEnumerable<object[]> GetParametersFor_GetSet_PathToIncorrectSet()
    {
        string setFilesDirPath = @"_Assets\Sets";
        return new List<object[]>
        {
            new object[]
            {
                Path.Combine(setFilesDirPath, "Empty"),
                "empty_999.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "NotExist",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>()
                        {
                            ["FilePath"] = Path.Combine(setFilesDirPath, "Empty", "empty_999.txt")
                        }
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "Empty"),
                "empty_1.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "Empty",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>()
                        {
                            ["FilePath"] = Path.Combine(setFilesDirPath, "Empty", "empty_1.txt")
                        }
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "NoSemicolon"),
                "no_semicolon_1.txt",
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
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "NoSemicolon"),
                "no_semicolon_2.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "NoSemicolon",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "2" }
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "TooManySemicolons"),
                "too_many_semicolons_1.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "TooManySemicolons",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "14", ["NumOfSemicolons"] = "2" }
                    ),
                    new ErrorInfo(
                        errorKey: "TooManySemicolons",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "15", ["NumOfSemicolons"] = "4" }
                    ),
                    new ErrorInfo(
                        errorKey: "TooManySemicolons",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "16", ["NumOfSemicolons"] = "2" }
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "NoWords"),
                "no_words_1.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "NoWords",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "24" }
                    ),
                    new ErrorInfo(
                        errorKey: "NoWords",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "31" }
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "EmptyWord"),
                "empty_word_1.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "EmptyWord",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "6" }
                    ),
                    new ErrorInfo(
                        errorKey: "EmptyWord",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "10" }
                    ),
                    new ErrorInfo(
                        errorKey: "EmptyWord",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "10" }
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "TooLongWord"),
                "too_long_word_1.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "TooLongWord",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "30" }
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "NoTranslations"),
                "no_translations_1.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "NoTranslations",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "14" }
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "EmptyTranslation"),
                "empty_translation_1.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "EmptyTranslation",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "10" }
                    ),
                    new ErrorInfo(
                        errorKey: "EmptyTranslation",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "10" }
                    ),
                    new ErrorInfo(
                        errorKey: "EmptyTranslation",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "11" }
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "TooLongTranslation"),
                "too_long_translation_1.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "TooLongTranslation",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>() { ["LineNum"] = "22" }
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "ErrorsCombination"),
                "example_set_1.txt",
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
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "ErrorsCombination"),
                "example_set_2.txt",
                new List<ErrorInfo>()
                {
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
                    )
                }
            },
            new object[]
            {
                Path.Combine(setFilesDirPath, "ErrorsCombination"),
                "example_set_3.txt",
                new List<ErrorInfo>()
                {
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
