using System.Collections.Generic;
using System.IO;
using R.Systems.Lexica.FunctionalTests.Common.Settings;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.FunctionalTests.Sets.Queries.GetSets.Parameters;

internal static class GetSets_PathToDirWithIncorrectSets
{
    public static IEnumerable<object[]> Get()
    {
        return new List<object[]>
        {
            new object[]
            {
                Path.Combine(AssetsPaths.SetFilesDirPath, "NoSemicolon"),
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
                Path.Combine(AssetsPaths.SetFilesDirPath, "ErrorsCombination"),
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
