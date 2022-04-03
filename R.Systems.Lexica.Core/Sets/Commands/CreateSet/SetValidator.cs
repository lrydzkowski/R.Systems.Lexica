using R.Systems.Lexica.Core.Common.Exceptions;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Core.Sets.Commands.CreateSet;

public class SetValidator
{
    public void Validate(Set set)
    {
        List<ErrorInfo> errors = new();
        errors.AddRange(ValidateName(set.Name));
        errors.AddRange(ValidateEntries(set.Entries));
        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }

    private List<ErrorInfo> ValidateName(string name)
    {
        List<ErrorInfo> errors = new();
        if (string.IsNullOrEmpty(name))
        {
            errors.Add(new ErrorInfo(errorKey: "Empty", elementKey: "Set.Name"));
            return errors;
        }
        if (name.Length > 100)
        {
            errors.Add(new ErrorInfo(
                errorKey: "TooLong",
                elementKey: "SetName",
                data: new Dictionary<string, string> { ["Name"] = name }
            ));
        }
        return errors;
    }

    private List<ErrorInfo> ValidateEntries(List<Entry> entries)
    {
        List<ErrorInfo> errors = new();
        for (int i = 0; i < entries.Count; i++)
        {
            errors.AddRange(ValidateEntry(entries[i], i + 1));
        }
        return errors;
    }

    private List<ErrorInfo> ValidateEntry(Entry entry, int lineNum)
    {
        List<ErrorInfo> errors = new();
        errors.AddRange(ValidateWords(entry.Words, lineNum));
        errors.AddRange(ValidateTranslation(entry.Translations, lineNum));
        return errors;
    }

    private List<ErrorInfo> ValidateWords(List<string> words, int lineNum)
    {
        List<ErrorInfo> errors = new();
        if (words.Count == 0)
        {
            errors.Add(
                new ErrorInfo(
                    errorKey: "NoWords",
                    elementKey: "Set.Words",
                    data: new Dictionary<string, string>
                    {
                        ["LineNum"] = lineNum.ToString()
                    }
                )
            );
        }
        foreach (string word in words)
        {
            if (string.IsNullOrEmpty(word))
            {
                errors.Add(
                    new ErrorInfo(
                        errorKey: "EmptyWord",
                        elementKey: "Set.Words",
                        data: new Dictionary<string, string>
                        {
                            ["LineNum"] = lineNum.ToString()
                        }
                    )
                );
                continue;
            }
            if (word.Length > 400)
            {
                errors.Add(
                    new ErrorInfo(
                        errorKey: "TooLongWord",
                        elementKey: "Set.Words",
                        data: new Dictionary<string, string>
                        {
                            ["LineNum"] = lineNum.ToString()
                        }
                    )
                );
            }
        }
        return errors;
    }

    private List<ErrorInfo> ValidateTranslation(List<string> translations, int lineNum)
    {
        List<ErrorInfo> errors = new();
        if (translations.Count == 0)
        {
            errors.Add(
                new ErrorInfo(
                    errorKey: "NoTranslations",
                    elementKey: "Set.Translations",
                    data: new Dictionary<string, string>
                    {
                        ["LineNum"] = lineNum.ToString()
                    }
                )
            );
        }
        foreach (string translation in translations)
        {
            if (string.IsNullOrEmpty(translation))
            {
                errors.Add(
                    new ErrorInfo(
                        errorKey: "EmptyTranslation",
                        elementKey: "Set.Translations",
                        data: new Dictionary<string, string>
                        {
                            ["LineNum"] = lineNum.ToString()
                        }
                    )
                );
                continue;
            }
            if (translation.Length > 400)
            {
                errors.Add(
                    new ErrorInfo(
                        errorKey: "TooLongTranslation",
                        elementKey: "Set.Translations",
                        data: new Dictionary<string, string>
                        {
                            ["LineNum"] = lineNum.ToString()
                        }
                    )
                );
            }
        }
        return errors;
    }
}
