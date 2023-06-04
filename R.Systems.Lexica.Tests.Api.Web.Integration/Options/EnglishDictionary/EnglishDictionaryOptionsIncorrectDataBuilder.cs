using R.Systems.Lexica.Infrastructure.EnglishDictionary.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.EnglishDictionary;

internal class EnglishDictionaryOptionsIncorrectDataBuilder : IncorrectDataBuilderBase<EnglishDictionaryOptionsData>
{
    public static IEnumerable<object[]> Build()
    {
        return new List<object[]>
        {
            BuildParameters(
                1,
                new EnglishDictionaryOptionsData
                {
                    Host = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            EnglishDictionaryOptions.Position,
                            nameof(EnglishDictionaryOptions.Host)
                        )
                    }
                )
            ),
            BuildParameters(
                2,
                new EnglishDictionaryOptionsData
                {
                    Host = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            EnglishDictionaryOptions.Position,
                            nameof(EnglishDictionaryOptions.Host)
                        )
                    }
                )
            ),
            BuildParameters(
                3,
                new EnglishDictionaryOptionsData
                {
                    Path = ""
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            EnglishDictionaryOptions.Position,
                            nameof(EnglishDictionaryOptions.Path)
                        )
                    }
                )
            ),
            BuildParameters(
                4,
                new EnglishDictionaryOptionsData
                {
                    Path = "  "
                },
                BuildExpectedExceptionMessage(
                    new List<string>
                    {
                        BuildNotEmptyErrorMessage(
                            EnglishDictionaryOptions.Position,
                            nameof(EnglishDictionaryOptions.Path)
                        )
                    }
                )
            )
        };
    }
}
