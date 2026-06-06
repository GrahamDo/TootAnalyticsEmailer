using System.Text;
using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable MemberCanBePrivate.Global
// All properties must have public getters and setters, and all classes must be concrete, for serialisation to work

namespace TootAnalyticsEmailer.Models;

internal class MastodonStatus
{
    [JsonPropertyName("created_at")]
    public DateTime CreatedAtUtc { get; set; }
    public DateTime CreatedAt => CreatedAtUtc.ToLocalTime();
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
    [JsonPropertyName("reblog")]
    public MastodonStatus? Reblog { get; set; }
    [JsonPropertyName("tags")]
    public List<MastodonHashTag> HashTagsList { get; set; } = [];

    public string HashTags {
        get
        {
            var result = new StringBuilder();
            foreach (var ht in HashTagsList) 
                result.Append($"#{ht.Name} ");
            return result.ToString().Trim();
        }
    }

    [JsonPropertyName("media_attachments")]
    public List<MastodonId> AttachmentIds { get; set; } = [];

    public string HasAttachments => AttachmentIds.Any() ? "Yes" : "No";
    [JsonPropertyName("reblogs_count")]
    public uint Boosts { get; set; }
    [JsonPropertyName("favourites_count")]
    public uint Favourites { get; set; }
    [JsonPropertyName("replies_count")]
    public uint Replies { get; set; }
}