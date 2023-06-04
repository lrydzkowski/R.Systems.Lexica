using R.Systems.Lexica.Infrastructure.Db.Common.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.ConnectionStrings;

internal class ConnectionStringsOptionsData : ConnectionStringsOptions, IOptionsData
{
    public ConnectionStringsOptionsData()
    {
        AppPostgresDb =
            "Server=127.0.0.1;Database=r_systems_lexica;Port=5502;User Id=r_systems_lexica_user;Password=123";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection(string? parentPosition = null)
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(AppPostgresDb)}"] = AppPostgresDb
        };
    }
}
