using System.Text;
using TootAnalyticsEmailer.Models;

namespace TootAnalyticsEmailer;

internal class CsvGenerator
{
    public string GenerateFromStatuses(List<MastodonStatus> statuses)
    {
        var result = new StringBuilder();
        result.AppendLine("\"Created At\",\"Is Boost?\",\"URL\",\"Content\",\"Hashtags\",\"Attachments\",\"Boosts\",\"Favourites\",\"Replies\"");
        statuses.ForEach(s =>
        {
            var isBoost = s.Reblog != null;
            var url = isBoost ? s.Reblog.Url : s.Url;
            var content = isBoost ? s.Reblog.Content : s.Content;
            result.AppendLine(
                $"\"{s.CreatedAt:yyyy-MM-dd HH:mm:ss}\",\"{isBoost}\",\"{url}\",\"{content}\",\"{s.HashTags}\",\"{s.HasAttachments}\",\"{s.Boosts}\",\"{s.Favourites}\",\"{s.Replies}\"");
        });

        return result.ToString();
    }
}