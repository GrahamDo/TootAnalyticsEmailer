using System.Text;
using TootAnalyticsEmailer.Models;

namespace TootAnalyticsEmailer;

internal static class CsvGenerator
{
    public static string GenerateFromStatuses(List<MastodonStatus> statuses)
    {
        var result = new StringBuilder();
        result.AppendLine("\"Created At\",\"Is Boost?\",\"URL\",\"Content\",\"Hashtags\",\"Attachments\",\"Boosts\",\"Favourites\",\"Replies\"");
        foreach (var s in statuses)
        {
            var isBoost = s.Reblog != null;
            var url = isBoost ? s.Reblog?.Url : s.Url;
            var content = isBoost ? s.Reblog?.Content : s.Content;
            var boosts = isBoost ? s.Reblog?.Boosts : s.Boosts;
            var favourites = isBoost ? s.Reblog?.Favourites : s.Favourites;
            var replies = isBoost ? s.Reblog?.Replies : s.Replies;
            result.AppendLine(
                $"\"{s.CreatedAt:yyyy-MM-dd HH:mm:ss}\",\"{isBoost}\",\"{url}\",\"{content}\",\"{s.HashTags}\",\"{s.HasAttachments}\",\"{boosts}\",\"{favourites}\",\"{replies}\"");
        }

        return result.ToString();
    }
}