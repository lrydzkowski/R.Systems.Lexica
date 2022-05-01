using System.Collections.Generic;
using System.IO;
using R.Systems.Lexica.FunctionalTests.Common.Settings;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.FunctionalTests.Sets.Queries.GetSet.Parameters;

internal static class GetSet_PathToIncorrectSet
{
    public static IEnumerable<object[]> Get()
    {
        return new List<object[]>
        {
            new object[]
            {
                Path.Combine(AssetsPaths.SetFilesDirPath, "Empty"),
                "empty_999.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "NotExist",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>()
                        {
                            ["FilePath"] = Path.Combine(AssetsPaths.SetFilesDirPath, "Empty", "empty_999.txt")
                        }
                    )
                }
            },
            new object[]
            {
                Path.Combine(AssetsPaths.SetFilesDirPath, "Empty"),
                "empty_1.txt",
                new List<ErrorInfo>()
                {
                    new ErrorInfo(
                        errorKey: "Empty",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>()
                        {
                            ["FilePath"] = Path.Combine(AssetsPaths.SetFilesDirPath, "Empty", "empty_1.txt")
                        }
                    )
                }
            },
            new object[]
            {
                Path.Combine(AssetsPaths.SetFilesDirPath, "NoSemicolon"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "NoSemicolon"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "TooManySemicolons"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "NoWords"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "EmptyWord"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "TooLongWord"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "NoTranslations"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "EmptyTranslation"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "TooLongTranslation"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "ErrorsCombination"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "ErrorsCombination"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "ErrorsCombination"),
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
