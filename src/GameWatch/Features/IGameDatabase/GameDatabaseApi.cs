using System.Net;
using Flurl;
using Flurl.Http;
using GameWatch.Common.Models;
using GameWatch.Features.Auth;
using Polly;

namespace GameWatch.Features.IGameDatabase;

public class GameDatabaseApi : IGameDatabaseApi
{
    private readonly IConfiguration _configuration;
    private readonly ITwitchAccessTokenService _twitchAccessTokenService;

    public GameDatabaseApi(
        IConfiguration configuration,
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
        var url = "https://api.igdb.com/v4/games".SetQueryParams(
            new { fields = "*", where_id = id }
        );
        var result = await FetchApi<Game>(url);
        return result;
    }

    private async Task<T> FetchApi<T>(Url url)
    {
        var token = await _twitchAccessTokenService.GetTwitchAccessTokenAsync(false);
        var clientId = _configuration["Twitch:ClientId"];

        var authPolicy = Policy
            .HandleResult<FlurlHttpException>(r => r.StatusCode is (int)HttpStatusCode.Unauthorized)
            .RetryAsync(
                1,
                onRetry: async (ex, retryCount) =>
                {
                    token = await _twitchAccessTokenService.GetTwitchAccessTokenAsync(true);
                }
            );

        var response = authPolicy.ExecuteAsync(
            () =>
                url.WithHeader("Client_Id", clientId)
                    .WithOAuthBearerToken(token)
                    .PostAsync()
                    .ReceiveJson<T>()
        );

        return response;
    }

    /*
     *         var token = await _twitchAccessTokenService.GetTwitchAccessTokenAsync(false);

        var result = await url.WithHeader("Client_Id", clientId)
            .WithOAuthBearerToken(token)
            .PostAsync()
            .ReceiveJson<T>();
     */
}
