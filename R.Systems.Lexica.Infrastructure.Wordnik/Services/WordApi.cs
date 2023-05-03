using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Common.Api;
using R.Systems.Lexica.Infrastructure.Wordnik.Models;
using R.Systems.Lexica.Infrastructure.Wordnik.Options;
using RestSharp;
using System.Net;

namespace R.Systems.Lexica.Infrastructure.Wordnik.Services;

internal class WordApi
{
    public const int RetryCount = 3;

    private readonly string _sourceDictionaries = "ahd-5";

    private readonly WordnikOptions _options;
    private readonly ILogger<WordApi> _logger;
    private readonly IApiRetryPolicies<WordApi> _apiRetryPolicies;
    private readonly RestClient _client;

    public WordApi(
        IOptions<WordnikOptions> options,
        ILogger<WordApi> logger,
        IApiRetryPolicies<WordApi> apiRetryPolicies
    )
    {
        _options = options.Value;
        _logger = logger;
        _apiRetryPolicies = apiRetryPolicies;
        _client = new RestClient(new RestClientOptions(_options.ApiBaseUrl) { ThrowOnDeserializationError = false });
    }

    public async Task<List<DefinitionDto>> GetDefinitionsAsync(string word, CancellationToken cancellationToken)
    {
        RestRequest request = new(_options.DefinitionsUrl.Replace("{word}", word));
        request.AddQueryParameter("limit", "10");
        request.AddQueryParameter("includeRelated", false);
        request.AddQueryParameter("sourceDictionaries", _sourceDictionaries);
        request.AddQueryParameter("useCanonical", false);
        request.AddQueryParameter("includeTags", false);
        request.AddQueryParameter("api_key", _options.ApiKey);

        RestResponse<List<DefinitionDto>?> response =
            await _apiRetryPolicies.ExecuteWithRetryPolicyAndCacheAsync(
                async (_, c) => await _client.ExecuteAsync<List<DefinitionDto>?>(request, c),
                (response) => !response.IsSuccessful,
                RetryCount,
                $"definitions_{word}",
                cancellationToken
            );

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new List<DefinitionDto>();
        }

        HandleUnexpectedError(response);

        return response.Data!;
    }

    private void HandleUnexpectedError(RestResponse response)
    {
        if (response.IsSuccessful)
        {
            return;
        }

        throw new WordnikApiException(
            $"Wordnik API request failed. Error message: '{response.ErrorMessage}'",
            response.ErrorException
        );
    }
}
