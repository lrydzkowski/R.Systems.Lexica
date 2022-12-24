using R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Common.TestsCollections;

[CollectionDefinition(CollectionName)]
public class MainTestsCollection : ICollectionFixture<WebApiFactory>
{
    public const string CollectionName = "MainTests";
}
