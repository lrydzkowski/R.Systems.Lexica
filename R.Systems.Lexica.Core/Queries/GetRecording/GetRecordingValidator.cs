using FluentValidation;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Queries.GetRecording;

public class GetRecordingValidator : AbstractValidator<GetRecordingQuery>
{
    public GetRecordingValidator()
    {
        RuleFor(x => x.WordType).NotEqual(WordType.None).WithName(nameof(Entry.WordType));
    }
}
