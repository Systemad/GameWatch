using System.Text.Json.Serialization;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace GameWatch.Features.Auth;

public class TwitchBaseTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
}

public class TwitchRefreshTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    [JsonPropertyName("token_type")]
    public int TokenType { get; set; }
}

public class TwitchAccessTokenService : ITwitchAccessTokenService
{
    private readonly IOptions<TwitchOptions> _config;
    private string AccessToken = "";

    public TwitchAccessTokenService(IOptions<TwitchOptions> config)
    {
        _config = config;
    }

    public async Task<string> GetTwitchAccessTokenAsync(bool unauthorized)
    {
        var clientId = _config.Value.ClientId;
        var clientSecret = _config.Value.ClientSecret;

        if (string.IsNullOrWhiteSpace(clientId))
            throw new Exception("Twitch:ClientId is is empty!");

        if (string.IsNullOrWhiteSpace(clientSecret))
            throw new Exception("Twitch:ClientSecret is is empty!");

        if (string.IsNullOrWhiteSpace(AccessToken))
            await FetchAndSetTwitchAccessTokenAsync(clientId, clientSecret);

        if (unauthorized && !string.IsNullOrWhiteSpace(AccessToken))
            await RefreshAndSetTwitchAccessTokenAsync(clientId, clientSecret);

        return AccessToken;
    }

    private async Task RefreshAndSetTwitchAccessTokenAsync(string clientId, string clientSecret)
    {
        var fetchedToken = await "https://id.twitch.tv/oauth2/token"
            .SetQueryParam("client_id", clientId)
            .SetQueryParam("client_secret", clientSecret)
            .SetQueryParam("grant_type", "refresh_token")
            .SetQueryParam("refresh_token", AccessToken)
            //.AllowAnyHttpStatus()
            .PostAsync()
            .ReceiveJson<TwitchRefreshTokenResponse>();

        if (fetchedToken is not null && !string.IsNullOrWhiteSpace(fetchedToken.AccessToken))
            AccessToken = fetchedToken.AccessToken;
    }

    private async Task FetchAndSetTwitchAccessTokenAsync(string clientId, string clientSecret)
    {
        var fetchedToken = await "https://id.twitch.tv/oauth2/token"
            .SetQueryParam("client_id", clientId)
            .SetQueryParam("client_secret", clientSecret)
            .SetQueryParam("grant_type", "client_credentials")
            //.AllowAnyHttpStatus()
            .PostAsync()
            .ReceiveJson<TwitchBaseTokenResponse>();

        if (fetchedToken is not null && !string.IsNullOrWhiteSpace(fetchedToken.AccessToken))
            AccessToken = fetchedToken.AccessToken;
    }
}
