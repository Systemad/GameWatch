using System.Text.Json.Serialization;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

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
    private readonly IOptions<TwitchOptions> _config;

    public TwitchAccessTokenService(IMemoryCache memoryCache, IOptions<TwitchOptions> config)
    {
        _memoryCache = memoryCache;
        _config = config;
    }

    public async Task<string> GetTwitchAccessTokenAsync(bool unauthorized)
    {
        var clientId = _config.Value.ClientId;
        var clientSecret = _config.Value.ClientSecret;
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
