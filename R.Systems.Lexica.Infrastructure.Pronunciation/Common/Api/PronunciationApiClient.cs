using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using R.Systems.Lexica.Infrastructure.Pronunciation.Common.Options;
using RestSharp;

namespace R.Systems.Lexica.Infrastructure.Pronunciation.Common.Api;

internal interface IPronunciationApiClient
{
    Task<List<byte[]>> DownloadPronunciationsAsync(string word);
}

internal class PronunciationApiClient : IPronunciationApiClient
{
    public PronunciationApiClient(IOptions<PronunciationApiOptions> options)
    {
        Options = options.Value;
        ApiUrl = new Uri(Options.ApiUrl);
        RestClient = new RestClient(GetBaseUrl());
    }

    private PronunciationApiOptions Options { get; }
    private Uri ApiUrl { get; }
    private RestClient RestClient { get; }

    public async Task<List<byte[]>> DownloadPronunciationsAsync(string word)
    {
        RestRequest contentRequest = new(GetUrlPath(word));
        RestResponse contentResponse = await RestClient.ExecuteAsync(contentRequest);
        HandleUnexpectedError(contentResponse, contentResponse.ResponseUri?.ToString() ?? "");
        if (contentResponse.Content == null)
        {
            return new();
        }

        string? fileUrl = GetFileUrl(word, contentResponse.Content);
        if (fileUrl == null)
        {
            return new();
        }

        RestRequest fileRequest = new(fileUrl);
        byte[]? file = await RestClient.DownloadDataAsync(fileRequest);

        return file == null ? new() : new() { file };
    }

    private string GetBaseUrl()
    {
        return $"{ApiUrl.Scheme}://{ApiUrl.Host}";
    }

    private string GetUrlPath(string word)
    {
        return ApiUrl.PathAndQuery.Replace(PronunciationApiUrlPlaceholders.Word, word);
    }

    private void HandleUnexpectedError(RestResponse response, string url)
    {
        if (response.ErrorException == null)
        {
            return;
        }

        throw new PronunciationApiException(
            $"Unexpected error has occurred in getting a pronunciation record. Url = {url}",
            response.ErrorException
        );
    }

    private string? GetFileUrl(string word, string content)
    {
        string? fileUrl = null;
        HtmlDocument htmlDoc = new();
        htmlDoc.LoadHtml(content);
        foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//div[@class=\"pos-header dpos-h\"]"))
        {
            HtmlNode titleNode = node.SelectSingleNode(".//div[@class=\"di-title\"]/span/span");
            if (titleNode == null)
            {
                continue;
            }

            string? title = titleNode.InnerText?.Trim();
            if (title != word)
            {
                continue;
            }

            HtmlNode audioSourceNode =
                node.SelectSingleNode(".//span[@class=\"us dpron-i \"]//source[@type=\"audio/mpeg\"]");
            if (audioSourceNode == null)
            {
                continue;
            }

            string src = audioSourceNode.GetAttributeValue<string>("src", def: "");
            if (string.IsNullOrEmpty(src))
            {
                continue;
            }

            fileUrl = src;
        }

        return fileUrl;
    }
}
