using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Tests.Integration.Common.Factories;
using R.Systems.Lexica.WebApi;

namespace R.Systems.Lexica.Tests.Integration.AutoMapper;

public class AutoMapperTests
{
    [Fact]
    public void AutoMapperConfiguration_ShouldBeValid()
    {
        WebApiFactory<Program> webApiFactory = new();
        IMapper? mapper = webApiFactory.Services.GetService<IMapper>();
        mapper?.ConfigurationProvider.AssertConfigurationIsValid();

        Assert.NotNull(mapper);
    }
}
