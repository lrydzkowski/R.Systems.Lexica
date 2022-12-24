using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.AutoMapper;

[Collection(MainTestsCollection.CollectionName)]
[Trait(TestConstants.Category, MainTestsCollection.CollectionName)]
public class AutoMapperTests : IClassFixture<WebApiFactory>
{
    public AutoMapperTests(WebApiFactory webApiFactory)
    {
        WebApiFactory = webApiFactory;
    }

    private WebApiFactory WebApiFactory { get; }

    [Fact]
    public void AutoMapperConfiguration_ShouldBeValid()
    {
        IMapper? mapper = WebApiFactory.Services.GetService<IMapper>();
        mapper?.ConfigurationProvider.AssertConfigurationIsValid();

        Assert.NotNull(mapper);
    }
}
