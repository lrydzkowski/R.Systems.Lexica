using FluentValidation;

namespace R.Systems.Lexica.Infrastructure.EnglishDictionary.Options;

internal class EnglishDictionaryOptionsValidator : AbstractValidator<EnglishDictionaryOptions>
{
    public EnglishDictionaryOptionsValidator()
    {
        DefineHostValidator();
        DefinePathValidator();
    }

    private void DefineHostValidator()
    {
        RuleFor(x => x.Host)
            .NotEmpty()
            .WithName(nameof(EnglishDictionaryOptions.Host))
            .OverridePropertyName($"{EnglishDictionaryOptions.Position}.{nameof(EnglishDictionaryOptions.Host)}");
    }

    private void DefinePathValidator()
    {
        RuleFor(x => x.Path)
            .NotEmpty()
            .DependentRules(
                () => RuleFor(x => x.Path)
                    .Must((path) => path.Contains(Constants.WordPlaceholder))
                    .WithMessage($"'{{PropertyName}}' must contain placeholder '{Constants.WordPlaceholder}'.")
                    .WithName(nameof(EnglishDictionaryOptions.Path))
                    .OverridePropertyName(
                        $"{EnglishDictionaryOptions.Position}.{nameof(EnglishDictionaryOptions.Path)}"
                    )
            )
            .WithName(nameof(EnglishDictionaryOptions.Path))
            .OverridePropertyName($"{EnglishDictionaryOptions.Position}.{nameof(EnglishDictionaryOptions.Path)}");
    }
}
