using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Caching.Memory;

namespace GameWatch.Features.Auth;

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
        var token = "";

        if (unauthorized)
        {
            token = await "https://id.twitch.tv/oauth2/token"
                .SetQueryParam("client_id", clientId)
                .SetQueryParam("client_secret", clientSecret)
                .AllowAnyHttpStatus()
                .PostAsync()
                .ReceiveString();
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(
                TimeSpan.FromSeconds(5000000)
            );
            _memoryCache.Set("twitchToken", token, cacheEntryOptions);
            return token;
        }

        if (!_memoryCache.TryGetValue("twitchToken", out string hey))
            token = hey;

        return token;
    }
}
