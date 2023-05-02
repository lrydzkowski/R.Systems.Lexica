using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Common.Api;
using R.Systems.Lexica.Infrastructure.EnglishDictionary.Errors;
using R.Systems.Lexica.Infrastructure.EnglishDictionary.Options;
using RestSharp;
using System.Net;

namespace R.Systems.Lexica.Infrastructure.EnglishDictionary.Services;

internal interface IApiClient
{
    Task<string?> GetPageAsync(string word, CancellationToken cancellationToken);
    Task<byte[]?> DownloadFileAsync(string relativePath, CancellationToken cancellationToken);
}

internal class ApiClient : IApiClient
{
    public const int RetryCount = 3;

    private readonly EnglishDictionaryOptions _options;
    private readonly ILogger<ApiClient> _logger;
    private readonly IApiRetryPolicies _apiRetryPolicies;
    private readonly RestClient _client;

    public ApiClient(
        IOptions<EnglishDictionaryOptions> options,
        ILogger<ApiClient> logger,
        IApiRetryPolicies apiRetryPolicies
    )
    {
        _options = options.Value;
        _logger = logger;
        _apiRetryPolicies = apiRetryPolicies;
        _client = new RestClient(new RestClientOptions(_options.Host));
    }

    public async Task<string?> GetPageAsync(string word, CancellationToken cancellationToken)
    {
        string requestPath = _options.Path.Replace("{word}", word);
        RestRequest request = new(requestPath, Method.Get);
        RestResponse response = await _apiRetryPolicies.ExecuteWithRetryPolicyAsync(
            _client,
            request,
            _logger,
            RetryCount,
            cancellationToken
        );
        if (WasRedirected(requestPath, response))
        {
            _logger.LogWarning($"Request to '{requestPath}' was redirected to '{response.ResponseUri?.AbsolutePath}'.");

            return null;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogWarning($"'{response.ResponseUri?.AbsoluteUri}' returns not found.");

            return null;
        }

        HandleUnexpectedError(response);

        return response?.Content ?? null;
    }

    public async Task<byte[]?> DownloadFileAsync(string relativePath, CancellationToken cancellationToken)
    {
        RestRequest request = new(relativePath, Method.Get);
        RestResponse response = await _apiRetryPolicies.ExecuteWithRetryPolicyAsync(
            _client,
            request,
            _logger,
            RetryCount,
            cancellationToken
        );

        HandleUnexpectedError(response);

        return response.RawBytes;
    }

    private bool WasRedirected(string requestPath, RestResponse response)
    {
        return response.ResponseUri != null
               && !response.ResponseUri.AbsolutePath.Equals(requestPath, StringComparison.InvariantCultureIgnoreCase);
    }

    private void HandleUnexpectedError(RestResponse response)
    {
        if (response.IsSuccessful)
        {
            return;
        }

        throw new EnglishDictionaryException(
            $"English Dictionary API request failed. Error message: '{response.ErrorMessage}'",
            response.ErrorException
        );
    }
}
