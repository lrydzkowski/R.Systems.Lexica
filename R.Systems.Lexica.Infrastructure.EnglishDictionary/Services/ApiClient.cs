using Microsoft.Extensions.Logging;
using R.Systems.Lexica.Infrastructure.EnglishDictionary.Errors;
using R.Systems.Lexica.Infrastructure.EnglishDictionary.Options;
using RestSharp;
using System.Net;
using Microsoft.Extensions.Options;

namespace R.Systems.Lexica.Infrastructure.EnglishDictionary.Services;

internal interface IApiClient
{
    Task<string?> GetPageAsync(string word);
    Task<byte[]?> DownloadFileAsync(string relativePath);
}

internal class ApiClient
    : IApiClient
{
    private readonly EnglishDictionaryOptions _options;
    private readonly ILogger<ApiClient> _logger;
    private readonly RestClient _client;

    public ApiClient(IOptions<EnglishDictionaryOptions> options, ILogger<ApiClient> logger)
    {
        _options = options.Value;
        _logger = logger;
        _client = new RestClient(new RestClientOptions(_options.Host));
    }

    public async Task<string?> GetPageAsync(string word)
    {
        string requestPath = _options.Path.Replace("{word}", word);
        RestRequest request = new(requestPath, Method.Get);
        RestResponse response = await _client.ExecuteAsync(request);
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

    public async Task<byte[]?> DownloadFileAsync(string relativePath)
    {
        RestRequest request = new(relativePath, Method.Get);
        RestResponse response = await _client.ExecuteAsync(request);

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
            $"Unexpected error has occurred in communication with English Dictionary API. Error message: '{response.ErrorMessage}'",
            response.ErrorException
        );
    }
}
