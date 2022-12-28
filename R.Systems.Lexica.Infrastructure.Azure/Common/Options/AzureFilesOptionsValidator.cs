using FluentValidation;

namespace R.Systems.Lexica.Infrastructure.Azure.Common.Options;

internal class AzureFilesOptionsValidator : AbstractValidator<AzureFilesOptions>
{
    public AzureFilesOptionsValidator()
    {
        DefineConnectionStringValidator();
        DefineFileShareNameValidator();
        DefineSetsDirectoryPathValidator();
        DefineRecordingsDirectoryPathValidator();
    }

    private void DefineConnectionStringValidator()
    {
        RuleFor(x => x.ConnectionString)
            .NotEmpty()
            .WithName(nameof(AzureFilesOptions.ConnectionString))
            .OverridePropertyName($"{AzureFilesOptions.Position}.{nameof(AzureFilesOptions.ConnectionString)}");
    }

    private void DefineFileShareNameValidator()
    {
        RuleFor(x => x.FileShareName)
            .NotEmpty()
            .WithName(nameof(AzureFilesOptions.FileShareName))
            .OverridePropertyName($"{AzureFilesOptions.Position}.{nameof(AzureFilesOptions.FileShareName)}");
    }

    private void DefineSetsDirectoryPathValidator()
    {
        RuleFor(x => x.SetsDirectoryPath)
            .NotEmpty()
            .WithName(nameof(AzureFilesOptions.SetsDirectoryPath))
            .OverridePropertyName($"{AzureFilesOptions.Position}.{nameof(AzureFilesOptions.SetsDirectoryPath)}");
    }

    private void DefineRecordingsDirectoryPathValidator()
    {
        RuleFor(x => x.RecordingsDirectoryPath)
            .NotEmpty()
            .WithName(nameof(AzureFilesOptions.RecordingsDirectoryPath))
            .OverridePropertyName($"{AzureFilesOptions.Position}.{nameof(AzureFilesOptions.RecordingsDirectoryPath)}");
    }
}
