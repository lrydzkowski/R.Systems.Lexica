using FluentValidation;

namespace R.Systems.Lexica.Infrastructure.Azure.Common.Options;

internal class AzureFilesOptionsValidator : AbstractValidator<AzureFilesOptions>
{
    public AzureFilesOptionsValidator()
    {
        RuleFor(x => x.ConnectionString)
            .NotEmpty()
            .WithName("ConnectionString")
            .OverridePropertyName($"{AzureFilesOptions.Position}.{nameof(AzureFilesOptions.ConnectionString)}");
        RuleFor(x => x.FileShareName)
            .NotEmpty()
            .WithName("FileShareName")
            .OverridePropertyName($"{AzureFilesOptions.Position}.{nameof(AzureFilesOptions.FileShareName)}");
    }
}
