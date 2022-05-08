using MediatR;
using R.Systems.Lexica.Core.Common.Models;
using System.Reflection;

namespace R.Systems.Lexica.Core.Version.Queries.GetVersion;

public class GetVersionQuery : IRequest<App>
{
}

public class GetVersionQueryHandler : IRequestHandler<GetVersionQuery, App>
{
    public Task<App> Handle(GetVersionQuery request, CancellationToken cancellationToken)
    {
        string? version = Assembly.GetEntryAssembly()?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.
            InformationalVersion;
        return Task.FromResult(new App { Version = version });
    }
}
