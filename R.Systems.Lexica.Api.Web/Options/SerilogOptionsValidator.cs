using FluentValidation;

namespace R.Systems.Lexica.Api.Web.Options;

internal class SerilogOptionsValidator : AbstractValidator<SerilogOptions>
{
    public SerilogOptionsValidator()
    {
        RuleFor(x => x.StorageAccount)
            .SetValidator(new SerilogStorageAccountValidator())
            .OverridePropertyName(SerilogOptions.Position);
    }
}

internal class SerilogStorageAccountValidator : AbstractValidator<SerilogStorageAccountOptions>
{
    public SerilogStorageAccountValidator()
    {
        RuleFor(x => x.ConnectionString)
            .NotEmpty()
            .WithName(nameof(SerilogStorageAccountOptions.ConnectionString))
            .OverridePropertyName(
                $"{SerilogStorageAccountOptions.Position}.{nameof(SerilogStorageAccountOptions.ConnectionString)}"
            );
        RuleFor(x => x.ContainerName)
            .NotEmpty()
            .WithName(nameof(SerilogStorageAccountOptions.ContainerName))
            .OverridePropertyName(
                $"{SerilogStorageAccountOptions.Position}.{nameof(SerilogStorageAccountOptions.ContainerName)}"
            );
    }
}
