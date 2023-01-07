using FluentValidation;

namespace R.Systems.Lexica.Infrastructure.Pronunciation.Common.Options;

internal class PronunciationApiOptionsValidator : AbstractValidator<PronunciationApiOptions>
{
    public PronunciationApiOptionsValidator()
    {
        DefineApiUrlValidator();
    }

    private void DefineApiUrlValidator()
    {
        RuleFor(x => x.ApiUrl)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(x => x.Contains(PronunciationApiUrlPlaceholders.Word))
            .WithMessage($"{{PropertyName}} must contain {PronunciationApiUrlPlaceholders.Word}.")
            .WithName(nameof(PronunciationApiOptions.ApiUrl))
            .OverridePropertyName($"{PronunciationApiOptions.Position}.{nameof(PronunciationApiOptions.ApiUrl)}");
    }
}
