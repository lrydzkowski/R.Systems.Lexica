using System.Text.Json.Serialization;

namespace R.Systems.Lexica.Infrastructure.Forvo.Common.Models;

internal class WordPronunciationResponse
{
    public List<Item> Items { get; init; } = new();
}

internal class Item
{
    [JsonPropertyName("pathmp3")] public string? PathMp3 { get; init; }
}
