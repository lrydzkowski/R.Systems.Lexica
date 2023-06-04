using R.Systems.Lexica.Infrastructure.Azure.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.AzureAd;

internal class AzureAdOptionsData : AzureAdOptions, IOptionsData
{
    public AzureAdOptionsData()
    {
        Instance = "https://login.microsoftonline.com/";
        ClientId = "F5BB96F4-B901-4DD6-8903-0D2A7BB514CC";
        TenantId = "59DC9E7F-5A22-4B18-A6A0-C5C485690694";
        Audience = "https://lrspaceb2c.onmicrosoft.com/B24CC390-56DF-4D42-9601-BFCEA8503611";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection(string? parentPosition = null)
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(Instance)}"] = Instance,
            [$"{Position}:{nameof(ClientId)}"] = ClientId,
            [$"{Position}:{nameof(TenantId)}"] = TenantId,
            [$"{Position}:{nameof(Audience)}"] = Audience
        };
    }
}
