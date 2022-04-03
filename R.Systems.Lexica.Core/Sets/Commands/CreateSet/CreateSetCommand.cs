using AutoMapper;
using MediatR;
using R.Systems.Lexica.Core.Common.Models;

namespace R.Systems.Lexica.Core.Sets.Commands.CreateSet;

public class CreateSetCommand : IRequest
{
    public string Name { get; init; } = "";

    public List<CreateSetEntryCommand> Entries { get; set; } = new();
}

public class CreateSetCommandHandler : IRequestHandler<CreateSetCommand>
{
    public CreateSetCommandHandler(ICreateSetRepository createSetRepository, IMapper mapper, SetValidator setValidator)
    {
        CreateSetRepository = createSetRepository;
        Mapper = mapper;
        SetValidator = setValidator;
    }

    public ICreateSetRepository CreateSetRepository { get; }
    public IMapper Mapper { get; }
    public SetValidator SetValidator { get; }

    public async Task<Unit> Handle(CreateSetCommand request, CancellationToken cancellationToken)
    {
        Set set = Mapper.Map<Set>(request);
        SetValidator.Validate(set);
        await CreateSetRepository.CreateSetAsync(set);
        return Unit.Value;
    }
}
