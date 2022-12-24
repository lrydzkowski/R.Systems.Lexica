﻿using FluentAssertions;
using R.Systems.Lexica.Core.App.Queries.GetAppInfo;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.ExceptionMiddleware;

[Collection(MainTestsCollection.CollectionName)]
[Trait(TestConstants.Category, MainTestsCollection.CollectionName)]
public class ExceptionMiddlewareTests : IClassFixture<WebApiFactory>
{
    public ExceptionMiddlewareTests(WebApiFactory webApiFactory)
    {
        WebApiFactory = webApiFactory;
    }

    private WebApiFactory WebApiFactory { get; }

    [Fact]
    public async Task GetAppInfo_ShouldReturn500InternalServerError_WhenUnexpectedExceptionWasThrown()
    {
        RestClient restClient = WebApiFactory.BuildWithCustomGetAppInfoHandler();

        RestRequest request = new("/");

        RestResponse response = await restClient.ExecuteAsync<GetAppInfoResult>(request);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Content.Should().Be("");
    }
}
