using System.Text;
using Newtonsoft.Json;

namespace TootAnalyticsEmailer.Models;

internal class MastodonStatus
{
    [JsonProperty("created_at")]
    public DateTime CreatedAtUtc { get; set; }
    public DateTime CreatedAt => CreatedAtUtc.ToLocalTime();
    public string Url { get; set; }
    public string Content { get; set; }
    [JsonProperty("tags")]
    public List<MastodonHashTag> HashTagsList { get; set; }
    public string HashTags {
        get
        {
            var result = new StringBuilder();
            HashTagsList.ForEach(ht => result.Append($"#{ht.Name} "));
            return result.ToString().Trim();
        }
    }

    [JsonProperty("media_attachments")]
    public List<MastodonId> AttachmentIds { get; set; }
    public string HasAttachments => AttachmentIds.Any() ? "Yes" : "No";
    [JsonProperty("reblogs_count")]
    public uint Boosts { get; set; }
    [JsonProperty("favourites_count")]
    public uint Favourites { get; set; }
    [JsonProperty("replies_count")]
    public uint Replies { get; set; }

    public MastodonStatus()
    {
        Content = string.Empty;
        AttachmentIds = new List<MastodonId>();
        HashTagsList = new List<MastodonHashTag>();
    }
}