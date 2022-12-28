using FluentValidation;

namespace R.Systems.Lexica.Infrastructure.Forvo.Common.Options;

internal class ForvoApiOptionsValidator : AbstractValidator<ForvoApiOptions>
{
    public ForvoApiOptionsValidator()
    {
        DefineApiKeyValidator();
        DefineApiUrlValidator();
    }

    private void DefineApiKeyValidator()
    {
        RuleFor(x => x.ApiKey)
            .NotEmpty()
            .WithName(nameof(ForvoApiOptions.ApiKey))
            .OverridePropertyName($"{ForvoApiOptions.Position}.{nameof(ForvoApiOptions.ApiKey)}");
    }

    private void DefineApiUrlValidator()
    {
        RuleFor(x => x.ApiUrl)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(x => x.Contains(ForvoApiUrlPlaceholders.ApiKey))
            .WithMessage($"{{PropertyName}} must contain {ForvoApiUrlPlaceholders.ApiKey}.")
            .Must(x => x.Contains(ForvoApiUrlPlaceholders.Word))
            .WithMessage($"{{PropertyName}} must contain {ForvoApiUrlPlaceholders.Word}.")
            .WithName(nameof(ForvoApiOptions.ApiUrl))
            .OverridePropertyName($"{ForvoApiOptions.Position}.{nameof(ForvoApiOptions.ApiUrl)}");
    }
}
