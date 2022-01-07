using R.Systems.Lexica.Core.Interfaces;
using R.Systems.Shared.Core.Interfaces;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Infrastructure.Validators;

public class SetValidator : IDependencyInjectionScoped
{
    public SetValidator(ValidationResult validationResult, ISetSource setSource)
    {
        ValidationResult = validationResult;
        SetSource = setSource;
    }

    public ValidationResult ValidationResult { get; }
    public ISetSource SetSource { get; }

    public bool SetDirExists(string dirPath)
    {
        if (!SetSource.DirExists(dirPath))
        {
            ValidationResult.Errors.Add(
                new ErrorInfo(
                    errorKey: "NotExist",
                    elementKey: "SetDir",
                    data: new Dictionary<string, string>() { ["DirPath"] = dirPath }
                )
            );
            return false;
        }
        return true;
    }

    public bool SetFileExists(string filePath)
    {
        if (!SetSource.Exists(filePath))
        {
            ValidationResult.Errors.Add(
                new ErrorInfo(
                    errorKey: "NotExist",
                    elementKey: "SetFile",
                    data: new Dictionary<string, string>() { ["FilePath"] = filePath }
                )
            );
            return false;
        }
        return true;
    }

    public bool ValidateSetFileContent(string? fileContent, string filePath)
    {
        fileContent = fileContent?.Trim();
        if (string.IsNullOrEmpty(fileContent))
        {
            ValidationResult.Errors.Add(
                new ErrorInfo(
                    errorKey: "Empty",
                    elementKey: "SetFile",
                    data: new Dictionary<string, string> { ["FilePath"] = filePath }
                )
            );
            return false;
        }
        bool result = true;
        string[] lines = fileContent.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            result &= ValidateLine(line, i + 1);
        }
        return result;
    }

    private bool ValidateLine(string line, int lineNum)
    {
        string[] parts = line.Split(';').Select(x => x.Trim()).ToArray();
        if (parts.Length == 1)
        {
            ValidationResult.Errors.Add(
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
            ValidationResult.Errors.Add(
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
        bool result = true;
        string wordsPart = parts[0].Trim();
        result &= ValidateWords(wordsPart, lineNum);
        string translationsPart = parts[1].Trim();
        result &= ValidateTranslations(translationsPart, lineNum);
        return result;
    }

    private bool ValidateWords(string wordsPart, int lineNum)
    {
        if (wordsPart.Length == 0)
        {
            ValidationResult.Errors.Add(
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
        bool result = true;
        List<string> words = wordsPart.Split(',').Select(x => x.Trim()).ToList();
        foreach (string word in words)
        {
            if (word.Length == 0)
            {
                ValidationResult.Errors.Add(
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
                ValidationResult.Errors.Add(
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
            ValidationResult.Errors.Add(
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
        bool result = true;
        List<string> translations = translationsPart.Split(',').Select(x => x.Trim()).ToList();
        foreach (string translation in translations)
        {
            if (translation.Length == 0)
            {
                ValidationResult.Errors.Add(
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
                ValidationResult.Errors.Add(
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
