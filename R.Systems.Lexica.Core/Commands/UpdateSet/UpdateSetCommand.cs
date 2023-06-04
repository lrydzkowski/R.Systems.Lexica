using MediatR;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Commands.UpdateSet;

public class UpdateSetCommand : IRequest<Unit>
{
    public long SetId { get; set; }

    public string SetName { get; set; } = "";

    public List<Entry> Entries { get; set; } = new();
}

public class UpdateSetCommandHandler : IRequestHandler<UpdateSetCommand, Unit>
{
    private readonly IUpdateSetRepository _repository;

    public UpdateSetCommandHandler(IUpdateSetRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateSetCommand command, CancellationToken cancellationToken)
    {
        await _repository.UpdateSetAsync(command);

        return Unit.Value;
    }
}
