using System.Net;
using Flurl;
using Flurl.Http;
using GameWatch.Features.Auth;
using GameWatch.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;

namespace GameWatch.Features.IGameDatabase;

// https://learn.microsoft.com/en-us/dotnet/core/extensions/caching
/*
 * TODO:
 * IDictiornary<body, cacheKey> Cachekeys:
 * Use body and cacheKey!
 * https://stackoverflow.com/questions/2190890/how-can-i-generate-a-guid-for-a-string!
 * Store object as cache!
 * Use entityframework to store??
 */
// TODO: Add caching!
// Store data!
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

    public async Task<Game[]> GetGamesByIdAsync(string[] ids)
    {
        //var query = new StringBuilder(50);
        //var games = string.Join(",", ids);
        //query.Append($"*; where id = ({games})");
        var body = $"fields *; where id = ({string.Join(",", ids)})";
        var url = "https://api.igdb.com/v4/games"; //.SetQueryParam("fields", query);
        var result = await FetchApi<Game[]>(url, body);
        return result;
    }

    public async Task<Game> GetGameAsync(string id)
    {
        var body = $"fields *; where id = {id}";
        var url = "https://api.igdb.com/v4/games"; //.SetQueryParam("fields", query);
        var result = await FetchApi<Game>(url, body);
        return result;
    }

    // Future: rename GetGamesByReleaseDateAsync and include full parameters
    // Split into separate library? Not priority
    public async Task<Game[]> GetGamesByMonthAsync(int year, int month)
    {
        // *; where date > 1675256460 & date < 1677848460; sort date asc;
        // Specify dates in unix epoch
        var unixPre = new DateTime(year, month, 1).ConvertDateTimeToUnix();
        var unixPast = new DateTime(year, month, 31).ConvertDateTimeToUnix();

        var body = $"fields *; where date > {unixPre} & date < {unixPast}; sort date asc;";
        var url = "https://api.igdb.com/v4/release_dates"; //.SetQueryParam(query);

        var result = await FetchApi<Game[]>(url, body);
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
                    .ReceiveJson<T>()
        );

        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(
            TimeSpan.FromDays(30)
        );

        cacheKey = Guid.NewGuid();
        _cacheKeys.Add(body, cacheKey);
        _cache.Set(cacheKey, response, cacheEntryOptions);
        return response;
    }
}
