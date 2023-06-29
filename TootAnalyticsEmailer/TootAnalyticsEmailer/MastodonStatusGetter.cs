using TootAnalyticsEmailer.Models;

namespace TootAnalyticsEmailer;

internal class MastodonStatusGetter
{
    private readonly Settings _settings;
    private readonly MastodonApiClient _apiClient;

    public MastodonStatusGetter(Settings settings, MastodonApiClient apiClient)
    {
        _settings = settings;
        _apiClient = apiClient;
    }

    public async Task<List<MastodonStatus>> GetStatuses(DateTime fromDate, DateTime toDate)
    {
        await _apiClient.VerifyCredentials(_settings.InstanceUrl, _settings.Token);
        var accountId = await _apiClient.GetIdForAccountName(_settings.AccountName);
        var statuses = await _apiClient.GetStatusesForAccountId(accountId);
        return statuses.Where(s => s.CreatedAt >= fromDate && s.CreatedAt <= toDate).ToList();
    }
}