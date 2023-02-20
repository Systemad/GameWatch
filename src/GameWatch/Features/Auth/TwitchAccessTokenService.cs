using System.Text.Json.Serialization;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Caching.Memory;

namespace GameWatch.Features.Auth;

public class TwitchTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
}

public class TwitchAccessTokenService : ITwitchAccessTokenService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;

    public TwitchAccessTokenService(IMemoryCache memoryCache, IConfiguration configuration)
    {
        _memoryCache = memoryCache;
        _configuration = configuration;
    }

    public async Task<string> GetTwitchAccessTokenAsync(bool unauthorized)
    {
        var clientId = _configuration["Twitch:ClientId"];
        var clientSecret = _configuration["Twitch:ClientSecret"];
        TwitchTokenResponse token = new();

        if (unauthorized)
        {
            token = await "https://id.twitch.tv/oauth2/token"
                .SetQueryParam("client_id", clientId)
                .SetQueryParam("client_secret", clientSecret)
                //.AllowAnyHttpStatus()
                .PostAsync()
                .ReceiveJson<TwitchTokenResponse>();

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(
                TimeSpan.FromSeconds(token.ExpiresIn)
            );
            _memoryCache.Set("twitchToken", token, cacheEntryOptions);
            return token.AccessToken;
        }

        if (!_memoryCache.TryGetValue("twitchToken", out TwitchTokenResponse hey))
            token = hey;

        return token.AccessToken;
    }
}
