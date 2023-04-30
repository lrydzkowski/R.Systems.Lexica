using MediatR;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Commands.CreateSet;

public class CreateSetCommand : IRequest<CreateSetResult>
{
    public string SetName { get; set; } = "";

    public List<Entry> Entries { get; set; } = new();
}

public class CreateSetResult
{
    public long SetId { get; init; }
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
        long setId = await _createSetRepository.CreateSetAsync(command);

        return new CreateSetResult
        {
            SetId = setId
        };
    }
}
