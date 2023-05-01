using FluentValidation;

namespace R.Systems.Lexica.Infrastructure.Azure.Options;

public class AzureStorageOptionsValidator : AbstractValidator<AzureStorageOptions>
{
    public AzureStorageOptionsValidator()
    {
        RuleFor(x => x.Blob).SetValidator(new BlobValidator());
    }
}

public class BlobValidator : AbstractValidator<BlobOptions>
{
    public BlobValidator()
    {
        RuleFor(x => x.ConnectionString)
            .NotEmpty()
            .WithName(nameof(BlobOptions.ConnectionString))
            .OverridePropertyName(
                $"{AzureStorageOptions.Position}.{BlobOptions.Position}.{nameof(BlobOptions.ConnectionString)}"
            );
        RuleFor(x => x.ContainerName)
            .NotEmpty()
            .WithName(nameof(BlobOptions.ContainerName))
            .OverridePropertyName(
                $"{AzureStorageOptions.Position}.{BlobOptions.Position}.{nameof(BlobOptions.ContainerName)}"
            );
    }
}
