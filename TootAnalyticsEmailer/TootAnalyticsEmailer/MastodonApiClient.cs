using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using TootAnalyticsEmailer.Models;

namespace TootAnalyticsEmailer;

internal class MastodonApiClient
{
    private readonly string _token;
    private static readonly HttpClient _client = new();

    public MastodonApiClient(string instanceUrl, string token)
    {
        if (string.IsNullOrEmpty(instanceUrl))
            throw new ApplicationException("Please specify the instance URL");
        if (string.IsNullOrEmpty(token))
            throw new ApplicationException("Please specify the token");
        
        var baseAddress = BuildBaseUrl(instanceUrl);
        _client.BaseAddress = new Uri(baseAddress);   
        _token = token;
    }
    
    public async Task VerifyCredentials()
    {
        var request = GetHttpRequestMessage("apps/verify_credentials");
        using var response = await _client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.Unauthorized)
                throw new ApplicationException("Invalid Mastodon token");
            
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }
    }

    private HttpRequestMessage GetHttpRequestMessage(string requestUrl)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl)
        {
            Headers = { Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token) }
        };
        return request;
    }

    public async Task<string> GetIdForAccountName(string accountName)
    {
        if (string.IsNullOrEmpty(accountName))
            throw new ApplicationException("Please specify the account name");

        var request = GetHttpRequestMessage($"accounts/lookup?acct={accountName}");
        using var response = await _client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        CheckForNullContent(responseContent, "Lookup account");

        Debug.Assert(response != null);
        var account = JsonSerializer.Deserialize<MastodonId>(responseContent);
        return account?.Id ??
               throw new ApplicationException($"Couldn't get ID for account {accountName}");
    }

    private static string BuildBaseUrl(string instanceUrl)
    {
        var baseUrlSb = new StringBuilder();
        if (!instanceUrl.StartsWith("https://"))
            baseUrlSb.Append("https://");
        baseUrlSb.Append(instanceUrl);
        if (!instanceUrl.EndsWith("/"))
            baseUrlSb.Append("/");
        baseUrlSb.Append("api/v1/");
        return baseUrlSb.ToString();
    }

    private static void CheckForNullContent(string? content, string apiMethodName)
    {
        if (content == null)
            throw new InvalidOperationException($"{apiMethodName} API method returned nothing");
    }

    public async Task<List<MastodonStatus>> GetStatusesForAccountId(string accountId)
    {
        var request = GetHttpRequestMessage($"accounts/{accountId}/statuses?limit=40");
        using var response = await _client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        CheckForNullContent(responseContent, $"Get statuses for follower {accountId}");

        Debug.Assert(responseContent != null);
        var statuses = JsonSerializer.Deserialize<List<MastodonStatus>>(responseContent);
        return statuses ?? new List<MastodonStatus>();
    }
}