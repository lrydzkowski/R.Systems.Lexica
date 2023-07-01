using FluentValidation;
using R.Systems.Lexica.Core.Commands.CreateSet;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Commands.UpdateSet;

public class UpdateSetCommandValidator : AbstractValidator<UpdateSetCommand>
{
    private readonly ICreateSetRepository _createSetRepository;

    public UpdateSetCommandValidator(ICreateSetRepository createSetRepository)
    {
        _createSetRepository = createSetRepository;
        DefineSetIdValidation();
        DefineSetNameValidation();
        DefineEntriesValidation();
    }

    private void DefineSetIdValidation()
    {
        RuleFor(x => x.SetId).NotEmpty();
    }

    private void DefineSetNameValidation()
    {
        RuleFor(x => x.SetName)
            .NotEmpty()
            .MaximumLength(200)
            .DependentRules(DefineSetNameUniquenessValidation)
            .WithName(nameof(CreateSetCommand.SetName));
    }

    private void DefineSetNameUniquenessValidation()
    {
        RuleFor(x => x)
            .MustAsync(
                async (setCommand, cancellationToken) =>
                {
                    bool setWithNameExists =
                        await _createSetRepository.SetExistsAsync(
                            setCommand.SetName,
                            setCommand.SetId,
                            cancellationToken
                        );

                    return !setWithNameExists;
                }
            )
            .WithName(nameof(CreateSetCommand.SetName))
            .WithMessage("'{PropertyName}' with the given name ('{PropertyValue}') exists.")
            .WithErrorCode("UniqueValidator");
        ;
    }

    private void DefineEntriesValidation()
    {
        RuleFor(x => x.Entries)
            .NotEmpty()
            .Must(
                (entries) =>
                {
                    List<string> distinctWords = entries.Select(x => x.Word).Distinct().ToList();

                    return distinctWords.Count == entries.Count;
                }
            )
            .WithMessage("'{PropertyName}' cannot contain repeated words.")
            .WithErrorCode("UniqueValidator");
        RuleForEach(x => x.Entries).SetValidator(new EntryValidator());
    }
}

public class EntryValidator : AbstractValidator<Entry>
{
    public EntryValidator()
    {
        RuleFor(x => x.Word).NotEmpty().MaximumLength(200);
        RuleFor(x => x.WordType).NotEqual(WordType.None).WithName(nameof(Entry.WordType));
        RuleFor(x => x.Translations)
            .NotEmpty()
            .DependentRules(() => RuleForEach(x => x.Translations).NotEmpty().MaximumLength(200));
    }
}
