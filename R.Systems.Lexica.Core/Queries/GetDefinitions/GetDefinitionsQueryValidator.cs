using FluentValidation;

namespace R.Systems.Lexica.Core.Queries.GetDefinitions;

public class GetDefinitionsQueryValidator : AbstractValidator<GetDefinitionsQuery>
{
    public GetDefinitionsQueryValidator()
    {
        RuleFor(x => x.Word).NotEmpty().MaximumLength(200);
    }
}
