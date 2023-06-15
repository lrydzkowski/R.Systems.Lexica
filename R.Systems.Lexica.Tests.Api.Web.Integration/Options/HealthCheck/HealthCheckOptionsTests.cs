﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc.Testing;
using R.Systems.Lexica.Api.Web;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;
using Xunit.Abstractions;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.HealthCheck;

[Collection(StandardCollection.CollectionName)]
[Trait(TestConstants.Category, StandardCollection.CollectionName)]
public class HealthCheckOptionsTests
{
    public HealthCheckOptionsTests(ITestOutputHelper output, WebApiFactory webApiFactory)
    {
        Output = output;
        WebApiFactory = webApiFactory.MockDirectoryExists();
    }

    private ITestOutputHelper Output { get; }
    private WebApplicationFactory<Program> WebApiFactory { get; }

    [Theory]
    [MemberData(
        nameof(HealthCheckOptionsIncorrectDataBuilder.Build),
        MemberType = typeof(HealthCheckOptionsIncorrectDataBuilder)
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
