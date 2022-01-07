using R.Systems.Lexica.FunctionalTests.Services;

namespace R.Systems.Lexica.FunctionalTests.Tests.SetTests;

public class SetControllerTests
{
    protected const string SetsUrl = "/sets";

    public SetControllerTests()
    {
        RequestService = new RequestService();
        AuthenticatorService = new AuthenticatorService();
        EmbeddedRsaKeys = new EmbeddedRsaKeys();
    }

    public RequestService RequestService { get; }
    public AuthenticatorService AuthenticatorService { get; }
    internal EmbeddedRsaKeys EmbeddedRsaKeys { get; }
}
