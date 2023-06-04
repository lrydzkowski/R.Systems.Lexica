using FluentValidation;

namespace R.Systems.Lexica.Infrastructure.Azure.Options;

internal class AzureStorageOptionsValidator : AbstractValidator<AzureStorageOptions>
{
    public AzureStorageOptionsValidator()
    {
        RuleFor(x => x.Blob)
            .SetValidator(new AzureStorageBlobValidator())
            .OverridePropertyName(AzureStorageOptions.Position);
    }
}

internal class AzureStorageBlobValidator : AbstractValidator<AzureStorageBlobOptions>
{
    public AzureStorageBlobValidator()
    {
        RuleFor(x => x.ConnectionString)
            .NotEmpty()
            .WithName(nameof(AzureStorageBlobOptions.ConnectionString))
            .OverridePropertyName(
                $"{AzureStorageBlobOptions.Position}.{nameof(AzureStorageBlobOptions.ConnectionString)}"
            );
        RuleFor(x => x.ContainerName)
            .NotEmpty()
            .WithName(nameof(AzureStorageBlobOptions.ContainerName))
            .OverridePropertyName(
                $"{AzureStorageBlobOptions.Position}.{nameof(AzureStorageBlobOptions.ContainerName)}"
            );
    }
}
