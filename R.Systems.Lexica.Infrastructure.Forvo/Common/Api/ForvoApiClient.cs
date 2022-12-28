using Microsoft.Extensions.Options;
using R.Systems.Lexica.Infrastructure.Forvo.Common.Models;
using R.Systems.Lexica.Infrastructure.Forvo.Common.Options;
using RestSharp;

namespace R.Systems.Lexica.Infrastructure.Forvo.Common.Api;

internal interface IForvoApiClient
{
    Task<List<byte[]>> DownloadPronunciationsAsync(string word);
}

internal class ForvoApiClient : IForvoApiClient
{
    public ForvoApiClient(IOptions<ForvoApiOptions> options)
    {
        Options = options.Value;
        ApiUrl = new Uri(Options.ApiUrl);
        RestClient = new RestClient(BuildAuthority());
    }

    private ForvoApiOptions Options { get; }
    private Uri ApiUrl { get; }
    private RestClient RestClient { get; }

    public async Task<List<byte[]>> DownloadPronunciationsAsync(string word)
    {
        RestRequest request = new(BuildAbsolutePath(word));

        RestResponse<WordPronunciationResponse> response =
            await RestClient.ExecuteAsync<WordPronunciationResponse>(request);
        HandleUnexpectedError(response, response.ResponseUri?.ToString() ?? "");

        if (response.Data == null)
        {
            return new();
        }

        List<string> urls = response.Data.Items.Select(item => item.PathMp3)
            .Where(url => url != null)
            .Cast<string>()
            .ToList();
        List<byte[]> files = new();
        foreach (string url in urls)
        {
            byte[]? file = await RestClient.DownloadDataAsync(new RestRequest(url));
            if (file == null)
            {
                throw new ForvoApiException($"ForvoApi has returned null instead of MP3 file. Url = {url}");
            }

            files.Add(file);
        }

        return files;
    }

    private string BuildAuthority()
    {
        return $"{ApiUrl.Scheme}://{ApiUrl.Host}";
    }

    private string BuildAbsolutePath(string word)
    {
        return new Uri(Options.ApiUrl).AbsolutePath.Replace(ForvoApiUrlPlaceholders.ApiKey, Options.ApiKey)
            .Replace(ForvoApiUrlPlaceholders.Word, word);
    }

    private static void HandleUnexpectedError<T>(RestResponse<T> response, string url)
    {
        if (response.ErrorException == null)
        {
            return;
        }

        throw new ForvoApiException(
            $"Unexpected error has occurred in getting links to pronunciation records. Url = {url}",
            response.ErrorException
        );
    }
}
