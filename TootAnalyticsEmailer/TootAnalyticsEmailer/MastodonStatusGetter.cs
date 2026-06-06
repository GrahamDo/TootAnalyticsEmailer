using TootAnalyticsEmailer.Models;

namespace TootAnalyticsEmailer;

internal class MastodonStatusGetter(Settings mainSettings, MastodonApiClient mainApiClient)
{
    public async Task<List<MastodonStatus>> GetStatuses(DateTime fromDate, DateTime toDate)
    {
        await mainApiClient.VerifyCredentials(mainSettings.InstanceUrl, mainSettings.Token);
        var accountId = await mainApiClient.GetIdForAccountName(mainSettings.AccountName, mainSettings.InstanceUrl, mainSettings.Token);
        var statuses = await mainApiClient.GetStatusesForAccountId(accountId, mainSettings.InstanceUrl, mainSettings.Token);
        return statuses.Where(s => s.CreatedAt >= fromDate && s.CreatedAt <= toDate).ToList();
    }
}