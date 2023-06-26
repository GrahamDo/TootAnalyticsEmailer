using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System.Text;

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
}