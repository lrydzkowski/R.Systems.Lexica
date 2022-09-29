using Microsoft.AspNetCore.Mvc.Testing;

namespace R.Systems.Lexica.Tests.Integration.Common.Factories;

public class WebApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
}
