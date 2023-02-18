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
        var reslult = await FetchApi(url);
        return reslult;
    }

    // https://flurl.dev/docs/configuration/
    // maybe handle event??
    // Polly to retry??
    // 401 request, trycatch handles and sets new accesstoken
    // Polly / flurl tries request again
    private async Task<Game[]> FetchApi(Url url)
    {
        var clientId = _configuration["Twitch:ClientId"];
        var accessToken = "";
        var token = "Bearer" + " " + accessToken;
        try
        {
            var result = await url.WithHeader("Client-ID", clientId)
                .WithHeader("Authorization", token)
                .PostAsync()
                .ReceiveJson<Game[]>();
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
