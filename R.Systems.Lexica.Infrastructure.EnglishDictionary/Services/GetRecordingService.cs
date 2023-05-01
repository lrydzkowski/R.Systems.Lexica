using HtmlAgilityPack;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Queries.GetRecording;

namespace R.Systems.Lexica.Infrastructure.EnglishDictionary.Services;

internal class GetRecordingService : IGetRecordingService
{
    private readonly IApiClient _apiClient;

    public GetRecordingService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<byte[]?> GetRecording(string word, WordType wordType)
    {
        string? pageContent = await _apiClient.GetPageAsync(word);
        if (pageContent == null)
        {
            return null;
        }

        string? link = GetLink(word, wordType, pageContent);
        if (link == null)
        {
            return null;
        }

        byte[]? recordingFile = await _apiClient.DownloadFileAsync(link);

        return recordingFile;
    }

    private string? GetLink(string word, WordType wordType, string pageContent)
    {
        string? link = null;

        HtmlDocument htmlDoc = new();
        htmlDoc.LoadHtml(pageContent);

        HtmlNodeCollection? wordNodes = htmlDoc.DocumentNode.SelectNodes("//span[contains(@class, 'headword')]");
        if (wordNodes == null)
        {
            return null;
        }

        foreach (HtmlNode? wordNode in wordNodes)
        {
            if (!wordNode.HasChildNodes)
            {
                continue;
            }

            string? foundWord = wordNode.ChildNodes[0].InnerText?.Trim().ToLower();
            if (foundWord == null || !word.Equals(foundWord, StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            HtmlNode? nextNode = wordNode.ParentNode.NextSibling;
            if (!nextNode.HasChildNodes)
            {
                continue;
            }

            string? foundWordTypeLabel = nextNode.ChildNodes[0].InnerText?.Trim().ToLower();
            if (foundWordTypeLabel == null)
            {
                continue;
            }

            WordType foundWordType = MapToWordType(foundWordTypeLabel);
            if (foundWordType == WordType.None || foundWordType != wordType)
            {
                continue;
            }

            link = wordNode.ParentNode.ParentNode.SelectSingleNode(".//span[contains(@class, 'us')]")
                ?.SelectSingleNode(".//source[@type='audio/mpeg']")
                ?.GetAttributeValue("src", null);
            if (link != null)
            {
                break;
            }
        }

        return link;
    }

    private WordType MapToWordType(string wordTypeLabel)
    {
        return wordTypeLabel switch
        {
            "noun" => WordType.Noun,
            "verb" => WordType.Verb,
            "adjective" => WordType.Adjective,
            "adverb" => WordType.Adverb,
            _ => WordType.None
        };
    }
}
