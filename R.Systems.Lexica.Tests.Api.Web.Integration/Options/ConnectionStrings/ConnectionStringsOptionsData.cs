using R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.ConnectionStrings;

internal class ConnectionStringsOptionsData : ConnectionStringsOptions, IOptionsData
{
    public ConnectionStringsOptionsData()
    {
        AppDb =
            "Server=127.0.0.1;Port=4044;Database=r-systems-lexica;User Id=r-systems-lexica;Password=rgre@#$2rewfgrRR;";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(AppDb)}"] = AppDb
        };
    }
}
