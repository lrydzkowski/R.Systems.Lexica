using R.Systems.Shared.Core.Interfaces;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Infrastructure.Validators;

public class SetValidator : IDependencyInjectionScoped
{
    public SetValidator(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }

    public ValidationResult ValidationResult { get; }

    public bool SetFileExists(string filePath)
    {
        if (!File.Exists(filePath))
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
            string[] parts = line.Split(';').Select(x => x.Trim()).ToArray();
            if (parts.Length == 1)
            {
                ValidationResult.Errors.Add(
                    new ErrorInfo(
                        errorKey: "NoSemicolon",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string> { ["LineNum"] = (i + 1).ToString() }
                    )
                );
                result = false;
                continue;
            }
            if (parts.Length > 2)
            {
                ValidationResult.Errors.Add(
                    new ErrorInfo(
                        errorKey: "TooManySemicolons",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>
                        {
                            ["LineNum"] = (i + 1).ToString(),
                            ["NumOfSemicolons"] = (parts.Length - 1).ToString()
                        }
                    )
                );
                result = false;
                continue;
            }
            if (parts[0].Length == 0)
            {
                ValidationResult.Errors.Add(
                    new ErrorInfo(
                        errorKey: "NoWords",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>
                        {
                            ["LineNum"] = (i + 1).ToString()
                        }
                    )
                );
                result = false;
            }
            if (parts[1].Length == 0)
            {
                ValidationResult.Errors.Add(
                    new ErrorInfo(
                        errorKey: "NoTranslations",
                        elementKey: "SetFile",
                        data: new Dictionary<string, string>
                        {
                            ["LineNum"] = (i + 1).ToString()
                        }
                    )
                );
                result = false;
            }
        }
        return result;
    }
}
