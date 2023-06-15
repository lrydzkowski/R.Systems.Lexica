using FluentValidation;
using R.Systems.Lexica.Infrastructure.Storage.Services;

namespace R.Systems.Lexica.Infrastructure.Storage.Options;

internal class StorageOptionsValidator : AbstractValidator<StorageOptions>
{
    private readonly IDirectoryHandler _directoryHandler;

    public StorageOptionsValidator(IDirectoryHandler directoryHandler)
    {
        _directoryHandler = directoryHandler;
        DefineDirectoryPathValidator();
    }

    private void DefineDirectoryPathValidator()
    {
        RuleFor(x => x.DirectoryPath)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(
                _directoryHandler.Exists
            )
            .WithMessage("'{PropertyName}' has to be a path to an existing directory.")
            .WithName(nameof(StorageOptions.DirectoryPath))
            .OverridePropertyName($"{StorageOptions.Position}.{nameof(StorageOptions.DirectoryPath)}");
    }
}
