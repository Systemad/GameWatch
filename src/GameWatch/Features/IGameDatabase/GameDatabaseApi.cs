using System.Net;
using Flurl;
using Flurl.Http;
using GameWatch.Common.Game;
using GameWatch.Features.Auth;
using GameWatch.Utilities;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;

namespace GameWatch.Features.IGameDatabase;

// TODO: Add caching!
// Store data!
public class GameDatabaseApi : IGameDatabaseApi
{
    private readonly IOptions<TwitchOptions> _configuration;
    private readonly ITwitchAccessTokenService _twitchAccessTokenService;

    public GameDatabaseApi(
        IOptions<TwitchOptions> configuration,
        ITwitchAccessTokenService twitchAccessTokenService
    )
    {
        _configuration = configuration;
        _twitchAccessTokenService = twitchAccessTokenService;
    }

    public async Task<Game[]> GetGamesAsync(string[] filters)
    {
        var url = "https://api.igdb.com/v4/games".SetQueryParams(new { fields = filters });
        var result = await FetchApi<Game[]>(url);
        return result;
    }

    public async Task<Game> GetGameAsync(string id)
    {
        var query = $"fields = *; where id = {id}";
        var url = "https://api.igdb.com/v4/games".SetQueryParam(query);
        var result = await FetchApi<Game>(url);
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

        var query = $"*; where date > {unixPre} & date < {unixPast}; sort date asc;";
        var url = "https://api.igdb.com/v4/release_dates".SetQueryParam(query);

        var result = await FetchApi<Game[]>(url);
        return result;
    }

    private async Task<T> FetchApi<T>(Url url)
    {
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
                url.WithHeader("Client_Id", clientId)
                    .WithOAuthBearerToken(token)
                    .PostAsync()
                    .ReceiveJson<T>()
        );

        return response;
    }
}
