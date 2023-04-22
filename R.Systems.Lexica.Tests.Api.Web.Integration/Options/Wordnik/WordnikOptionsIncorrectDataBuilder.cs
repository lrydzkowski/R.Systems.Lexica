using R.Systems.Lexica.Infrastructure.Wordnik.Common.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.Wordnik;

internal class WordnikOptionsIncorrectDataBuilder : IncorrectDataBuilderBase<WordnikOptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new WordnikOptionsData
                {
                    ApiBaseUrl = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            WordnikOptions.Position,
                            nameof(WordnikOptions.ApiBaseUrl)
                        )
                    }
                )
            ),
            BuildParameters(
                2,
                new WordnikOptionsData
                {
                    ApiBaseUrl = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            WordnikOptions.Position,
                            nameof(WordnikOptions.ApiBaseUrl)
                        )
                    }
                )
            ),
            BuildParameters(
                3,
                new WordnikOptionsData
                {
                    DefinitionsUrl = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            WordnikOptions.Position,
                            nameof(WordnikOptions.DefinitionsUrl)
                        )
                    }
                )
            ),
            BuildParameters(
                4,
                new WordnikOptionsData
                {
                    DefinitionsUrl = " "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            WordnikOptions.Position,
                            nameof(WordnikOptions.DefinitionsUrl)
                        )
                    }
                )
            ),
            BuildParameters(
                5,
                new WordnikOptionsData
                {
                    ApiKey = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            WordnikOptions.Position,
                            nameof(WordnikOptions.ApiKey)
                        )
                    }
                )
            ),
            BuildParameters(
                6,
                new WordnikOptionsData
                {
                    ApiKey = " "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            WordnikOptions.Position,
                            nameof(WordnikOptions.ApiKey)
                        )
                    }
                )
            )
        };
    }
}
