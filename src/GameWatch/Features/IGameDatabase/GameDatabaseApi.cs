using System.Net;
using System.Text.Json;
using Flurl;
using Flurl.Http;
using GameWatch.Features.Auth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;

namespace GameWatch.Features.IGameDatabase;

public class GameDatabaseApi : IGameDatabaseApi
{
    private readonly IOptions<TwitchOptions> _configuration;
    private readonly ITwitchAccessTokenService _twitchAccessTokenService;
    private readonly IMemoryCache _cache;
    private Dictionary<string, Guid> _cacheKeys = new();

    public GameDatabaseApi(
        IOptions<TwitchOptions> configuration,
        ITwitchAccessTokenService twitchAccessTokenService,
        IMemoryCache cache
    )
    {
        _configuration = configuration;
        _twitchAccessTokenService = twitchAccessTokenService;
        _cache = cache;
    }

    public async Task<T> GenericFetch<T>(string url, string body)
    {
        var result = await FetchApi<T>(url, body);
        return result;
    }

    private async Task<T> FetchApi<T>(Url url, string body)
    {
        if (_cacheKeys.TryGetValue(body, out Guid cacheKey))
        {
            if (_cache.TryGetValue(cacheKey, out T cacheValue))
            {
                return cacheValue;
            }
        }

        var token = await _twitchAccessTokenService.GetTwitchAccessTokenAsync(false);
        var clientId = _configuration.Value.ClientId;

        var authPolicy = Policy
            .Handle<FlurlHttpException>(r => r.StatusCode is (int)HttpStatusCode.Unauthorized)
            .RetryAsync(
                1,
                onRetry: async (_, _) =>
                {
                    Log.Information("IGDB authorization failed, refreshing new token");
                    token = await _twitchAccessTokenService.GetTwitchAccessTokenAsync(true);
                }
            );

        var response = await authPolicy.ExecuteAsync(
            () =>
                url.WithHeader("Content-Type", "text/plain")
                    .WithHeader("Client-Id", clientId)
                    .WithOAuthBearerToken(token)
                    .PostAsync(new StringContent(body))
                    .ReceiveString()
        );

        // Workaround, flurl receiving a type directly does not serialize properly
        var deserialize = JsonSerializer.Deserialize<T>(response);
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(
            TimeSpan.FromDays(30)
        );
        //cacheKey = Guid.NewGuid();
        //_cacheKeys.Add(body, cacheKey);
        //_cache.Set(cacheKey, response, cacheEntryOptions);
        return deserialize;
    }
}
