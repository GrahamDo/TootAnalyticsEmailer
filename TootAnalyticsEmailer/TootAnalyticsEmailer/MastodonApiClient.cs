using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System.Diagnostics;
using System.Text;
using TootAnalyticsEmailer.Models;

namespace TootAnalyticsEmailer;

internal class MastodonApiClient
{
    private RestClient _restClient = null!;

    public async Task VerifyCredentials(string instanceUrl, string token)
    {
        if (string.IsNullOrEmpty(instanceUrl))
            throw new ApplicationException("Please specify the instance URL");
        if (string.IsNullOrEmpty(token))
            throw new ApplicationException("Please specify the token");

        var baseUrl = BuildBaseUrl(instanceUrl);
        var options = new RestClientOptions(baseUrl)
        {
            Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer")
        };
        _restClient = new RestClient(options);

        var request = new RestRequest($"apps/verify_credentials");
        try
        {
            await _restClient.GetAsync(request);
        }
        catch (HttpRequestException ex)
        {
            if (ex.Message.Contains("Unauthorized"))
                throw new ApplicationException("The token you entered was not valid");

            throw;
        }
    }

    public async Task<string> GetIdForAccountName(string accountName)
    {
        if (string.IsNullOrEmpty(accountName))
            throw new ApplicationException("Please specify the account name");

        var request = new RestRequest($"accounts/lookup?acct={accountName}");
        var response = await _restClient.GetAsync(request);
        CheckForNullContent(response.Content, "Lookup account");

        Debug.Assert(response.Content != null, "response.Content != null");
        var account = JsonConvert.DeserializeObject<MastodonId>(response.Content);
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
}