using FluentValidation;

namespace R.Systems.Lexica.Infrastructure.Auth0.Options;

internal class Auth0OptionsValidator : AbstractValidator<Auth0Options>
{
    public Auth0OptionsValidator()
    {
        DefineDomainValidator();
        DefineAudienceValidator();
    }

    private void DefineDomainValidator()
    {
        RuleFor(x => x.Domain)
            .NotEmpty()
            .WithName(nameof(Auth0Options.Domain))
            .OverridePropertyName($"{Auth0Options.Position}.{nameof(Auth0Options.Domain)}");
    }

    private void DefineAudienceValidator()
    {
        RuleFor(x => x.Audience)
            .NotEmpty()
            .WithName(nameof(Auth0Options.Audience))
            .OverridePropertyName($"{Auth0Options.Position}.{nameof(Auth0Options.Audience)}");
    }
}
