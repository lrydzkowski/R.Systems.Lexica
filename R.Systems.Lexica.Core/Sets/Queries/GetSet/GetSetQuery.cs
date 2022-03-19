using MediatR;
using R.Systems.Lexica.Core.Common.Models;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSet;

public class GetSetQuery : IRequest<Set>
{
    public string SetName { get; set; } = "";
}

public class GetSetQueryHandler : IRequestHandler<GetSetQuery, Set>
{
    public GetSetQueryHandler(IGetSetRepository repository)
    {
        Repository = repository;
    }

    public IGetSetRepository Repository { get; }

    public async Task<Set> Handle(GetSetQuery request, CancellationToken cancellationToken)
    {
        return await Repository.GetSetAsync(request.SetName);
    }
}
