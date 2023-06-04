using MediatR;

namespace R.Systems.Lexica.Core.Commands.DeleteSet;

public class DeleteSetCommand : IRequest
{
    public long SetId { get; init; }
}

public class DeleteSetCommandHandler : IRequestHandler<DeleteSetCommand>
{
    private readonly IDeleteSetRepository _deleteSetRepository;

    public DeleteSetCommandHandler(IDeleteSetRepository deleteSetRepository)
    {
        _deleteSetRepository = deleteSetRepository;
    }

    public async Task Handle(DeleteSetCommand command, CancellationToken cancellationToken)
    {
        await _deleteSetRepository.DeleteSetAsync(command.SetId);
    }
}
