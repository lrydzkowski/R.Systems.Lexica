using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using R.Systems.Lexica.FunctionalTests.Common.Initializers;
using R.Systems.Lexica.FunctionalTests.Common.Services;
using R.Systems.Lexica.FunctionalTests.Common.Settings;
using R.Systems.Lexica.WebApi;
using Xunit;

namespace R.Systems.Lexica.FunctionalTests.Common.Tests;

public class TestsBase
{
    public TestsBase()
    {
        RequestService = new RequestService();
        AuthenticatorService = new AuthenticatorService();
        EmbeddedRsaKeys = new EmbeddedRsaKeys();
    }

    protected string SetsUrl { get; } = "/sets";

    protected RequestService RequestService { get; }

    protected AuthenticatorService AuthenticatorService { get; }

    protected EmbeddedRsaKeys EmbeddedRsaKeys { get; }

    protected async Task SendRequestWithoutAuthenticationTokenAsync<T>(string url) where T : class
    {
        HttpClient httpClient = new TestWebApplicationFactory<Program>(AssetsPaths.SetCorrectFilesDirPath)
            .CreateClient();

        (HttpStatusCode httpStatusCode, T? sets) = await RequestService.SendGetAsync<T>(url, httpClient);

        Assert.Equal(HttpStatusCode.Unauthorized, httpStatusCode);
        Assert.Null(sets);
    }
}
