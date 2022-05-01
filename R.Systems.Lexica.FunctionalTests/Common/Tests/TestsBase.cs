using R.Systems.Lexica.FunctionalTests.Common.Services;

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
}
