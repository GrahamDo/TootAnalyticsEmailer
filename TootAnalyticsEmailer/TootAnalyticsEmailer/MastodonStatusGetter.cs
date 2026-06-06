using TootAnalyticsEmailer.Models;

namespace TootAnalyticsEmailer;

internal class MastodonStatusGetter(MastodonApiClient mainApiClient)
{
    public async Task<List<MastodonStatus>> GetStatuses(string accountName, DateTime fromDate, DateTime toDate)
    {
        await mainApiClient.VerifyCredentials();
        var accountId = await mainApiClient.GetIdForAccountName(accountName);
        var statuses = await mainApiClient.GetStatusesForAccountId(accountId);
        return statuses.Where(s => s.CreatedAt >= fromDate && s.CreatedAt <= toDate).ToList();
    }
}