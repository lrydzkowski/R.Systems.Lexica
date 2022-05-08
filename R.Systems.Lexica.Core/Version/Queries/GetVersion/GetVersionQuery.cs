using MediatR;
using System.Reflection;

namespace R.Systems.Lexica.Core.Version.Queries.GetVersion;

public class GetVersionQuery : IRequest<string?>
{
}

public class GetVersionQueryHandler : IRequestHandler<GetVersionQuery, string?>
{
    public Task<string?> Handle(GetVersionQuery request, CancellationToken cancellationToken)
    {
        string? version = Assembly.GetEntryAssembly()?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.
            InformationalVersion;
        return Task.FromResult(version);
    }
}
