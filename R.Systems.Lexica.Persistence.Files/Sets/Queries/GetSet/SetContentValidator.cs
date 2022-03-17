using R.Systems.Lexica.Core.Common.Exceptions;
using R.Systems.Lexica.Persistence.Files.Sets.Common;
using R.Systems.Shared.Core.Interfaces;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Persistence.Files.Sets.Queries.GetSet;

internal class SetContentValidator : IDependencyInjectionScoped
{
    public SetContentValidator(ISetSource setSource)
    {
        SetSource = setSource;
    }

    public ISetSource SetSource { get; }
    private List<ErrorInfo> Errors { get; set; } = new();

    public void ValidateContent(string? fileContent, string filePath)
    {
        Errors = new List<ErrorInfo>();
        fileContent = fileContent?.Trim();
        if (string.IsNullOrEmpty(fileContent))
        {
            ErrorInfo errorInfo = new(
                errorKey: "Empty",
                elementKey: "SetFile",
                data: new Dictionary<string, string> { ["FilePath"] = filePath }
            );
            throw new ValidationException(errorInfo);
        }
        var lines = fileContent.Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            ValidateLine(line, i + 1);
        }
        if (Errors.Count > 0)
        {
            throw new ValidationException(Errors);
        }
    }

    private bool ValidateLine(string line, int lineNum)
    {
        var parts = line.Split(';').Select(x => x.Trim()).ToArray();
        if (parts.Length == 1)
        {
            Errors.Add(
                new ErrorInfo(
                    errorKey: "NoSemicolon",
                    elementKey: "SetFile",
                    data: new Dictionary<string, string> { ["LineNum"] = lineNum.ToString() }
                )
            );
            return false;
        }
        if (parts.Length > 2)
        {
            Errors.Add(
                new ErrorInfo(
                    errorKey: "TooManySemicolons",
                    elementKey: "SetFile",
                    data: new Dictionary<string, string>
                    {
                        ["LineNum"] = lineNum.ToString(),
                        ["NumOfSemicolons"] = (parts.Length - 1).ToString()
                    }
                )
            );
            return false;
        }
        var result = true;
        var wordsPart = parts[0].Trim();
        result &= ValidateWords(wordsPart, lineNum);
        var translationsPart = parts[1].Trim();
        result &= ValidateTranslations(translationsPart, lineNum);
        return result;
    }

    private bool ValidateWords(string wordsPart, int lineNum)
    {
        if (wordsPart.Length == 0)
        {
            Errors.Add(
                new ErrorInfo(
                    errorKey: "NoWords",
                    elementKey: "SetFile",
                    data: new Dictionary<string, string>
                    {
                        ["LineNum"] = lineNum.ToString()
                    }
                )
            );
            return false;
        }
        var result = true;
        var words = wordsPart.Split(',').Select(x => x.Trim()).ToList();
        foreach (var word in words)
        {
            if (word.Length == 0)
            {
                Errors.Add(
                    new ErrorInfo(
                        errorKey: "EmptyWord",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>
                        {
                            ["LineNum"] = lineNum.ToString()
                        }
                    )
                );
                result = false;
                continue;
            }
            if (word.Length > 400)
            {
                Errors.Add(
                    new ErrorInfo(
                        errorKey: "TooLongWord",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>
                        {
                            ["LineNum"] = lineNum.ToString()
                        }
                    )
                );
                result = false;
                continue;
            }
        }
        return result;
    }

    private bool ValidateTranslations(string translationsPart, int lineNum)
    {
        if (translationsPart.Length == 0)
        {
            Errors.Add(
                new ErrorInfo(
                    errorKey: "NoTranslations",
                    elementKey: "SetFile",
                    data: new Dictionary<string, string>
                    {
                        ["LineNum"] = lineNum.ToString()
                    }
                )
            );
            return false;
        }
        var result = true;
        var translations = translationsPart.Split(',').Select(x => x.Trim()).ToList();
        foreach (var translation in translations)
        {
            if (translation.Length == 0)
            {
                Errors.Add(
                    new ErrorInfo(
                        errorKey: "EmptyTranslation",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>
                        {
                            ["LineNum"] = lineNum.ToString()
                        }
                    )
                );
                result = false;
                continue;
            }
            if (translation.Length > 400)
            {
                Errors.Add(
                    new ErrorInfo(
                        errorKey: "TooLongTranslation",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>
                        {
                            ["LineNum"] = lineNum.ToString()
                        }
                    )
                );
                result = false;
                continue;
            }
        }
        return result;
    }
}
