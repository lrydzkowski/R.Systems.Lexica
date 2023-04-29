using MediatR;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Commands.CreateSet;

public class CreateSetCommand : IRequest<CreateSetResult>
{
    public string SetName { get; init; } = "";

    public List<Entry> Entries { get; init; } = new();
}

public class CreateSetResult
{
}

public class CreateSetCommandHandler : IRequestHandler<CreateSetCommand, CreateSetResult>
{
    private readonly ICreateSetRepository _createSetRepository;

    public CreateSetCommandHandler(ICreateSetRepository createSetRepository)
    {
        _createSetRepository = createSetRepository;
    }

    public async Task<CreateSetResult> Handle(CreateSetCommand command, CancellationToken cancellationToken)
    {
        await _createSetRepository.CreateSetAsync(command);

        return new CreateSetResult();
    }
}
