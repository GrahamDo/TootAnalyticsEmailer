using System.Text;
using TootAnalyticsEmailer.Models;

namespace TootAnalyticsEmailer;

internal class CsvGenerator
{
    public string GenerateFromStatuses(List<MastodonStatus> statuses)
    {
        var result = new StringBuilder();
        result.AppendLine("\"Created At\",\"Content\",\"Hashtags\",\"Boosts\",\"Favourites\",\"Replies\"");
        statuses.ForEach(s =>
        {
            result.AppendLine(
                $"\"{s.CreatedAt}\",\"{s.Content}\",\"{s.HashTags}\",\"{s.Boosts}\",\"{s.Favourites}\",\"{s.Replies}\"");
        });

        return result.ToString();
    }
}