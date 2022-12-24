﻿using FluentValidation;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;
using Xunit.Abstractions;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.AzureFiles;

[Collection(MainTestsCollection.CollectionName)]
[Trait(TestConstants.Category, MainTestsCollection.CollectionName)]
public class AzureFilesOptionsTests : IClassFixture<WebApiFactory>
{
    public AzureFilesOptionsTests(ITestOutputHelper output, WebApiFactory webApiFactory)
    {
        Output = output;
        WebApiFactory = webApiFactory;
    }

    private ITestOutputHelper Output { get; }
    private WebApiFactory WebApiFactory { get; }

    [Theory]
    [MemberData(
        nameof(AzureFilesOptionsIncorrectDataBuilder.Build),
        MemberType = typeof(AzureFilesOptionsIncorrectDataBuilder)
    )]
    public void InitApp_IncorrectAppSettings_ThrowsException(
        int id,
        Dictionary<string, string?> options,
        string expectedErrorMessage
    )
    {
        Output.WriteLine("Parameters set with id = {0}", id);

        ValidationException ex = Assert.Throws<ValidationException>(
            () => WebApiFactory.WithCustomOptions(options).CreateRestClient()
        );

        Assert.Equal(expectedErrorMessage, ex.Message);
    }
}
