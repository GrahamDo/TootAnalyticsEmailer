using System.Text;
using TootAnalyticsEmailer.Models;

namespace TootAnalyticsEmailer;

internal class CsvGenerator
{
    public string GenerateFromStatuses(List<MastodonStatus> statuses)
    {
        var result = new StringBuilder();
        result.AppendLine("\"Created At\",\"URL\",\"Content\",\"Hashtags\",\"Attachments\",\"Boosts\",\"Favourites\",\"Replies\"");
        statuses.ForEach(s =>
        {
            var url = s.Reblog != null ? s.Reblog.Url : s.Url;
            var content = s.Reblog != null ? s.Reblog.Content : s.Content;
            result.AppendLine(
                $"\"{s.CreatedAt}\",\"{url}\",\"{content}\",\"{s.HashTags}\",\"{s.HasAttachments}\",\"{s.Boosts}\",\"{s.Favourites}\",\"{s.Replies}\"");
        });

        return result.ToString();
    }
}