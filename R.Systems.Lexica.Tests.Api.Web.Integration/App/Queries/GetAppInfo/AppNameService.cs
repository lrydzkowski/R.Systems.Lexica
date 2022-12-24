using R.Systems.Lexica.Api.Web;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.App.Queries.GetAppInfo;

internal static class AppNameService
{
    public static string GetWebApiName()
    {
        return typeof(Program).Namespace ?? "";
    }
}
