using Flurl;
using Flurl.Http;
using GameWatch.Common.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GameWatch.Features.IGameDatabase;

public class GameDatabaseApi : IGameDatabaseApi
{
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;

    public GameDatabaseApi(IMemoryCache memoryCache, IConfiguration configuration)
    {
        _memoryCache = memoryCache;
        _configuration = configuration;
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
        var clientId = _configuration["Twitch:ClientId"];
        var accessToken = "";
        try
        {
            var result = await url.WithHeader("Client_Id", clientId)
                .WithOAuthBearerToken(accessToken)
                .PostAsync()
                .ReceiveJson<T>();
            return result;
        }
        catch (FlurlHttpException exception)
        {
            // Expire token / Unauthorized, refresh token
            if (exception.StatusCode == 401)
            {
                var tok = new TwitchAccessToken(_memoryCache, _configuration);
                await tok.FetchTwitchAccessToken();
            }
            throw;
        }
    }
}
