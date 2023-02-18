using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Caching.Memory;

namespace GameWatch.Features.IGameDatabase;

public class TwitchAccessToken
{
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;

    public TwitchAccessToken(IMemoryCache memoryCache, IConfiguration configuration)
    {
        _memoryCache = memoryCache;
        _configuration = configuration;
    }

    public async Task FetchTwitchAccessToken()
    {
        var clientId = _configuration["Twitch:ClientId"];
        var clientSecret = _configuration["Twitch:ClientSecret"];
        var token = await "https://id.twitch.tv/oauth2/token"
            .SetQueryParam("client_id", clientId)
            .SetQueryParam("client_secret", clientSecret)
            .AllowAnyHttpStatus()
            .PostAsync()
            .ReceiveString();
        _memoryCache.Set("twitchToken", token);
    }
}
