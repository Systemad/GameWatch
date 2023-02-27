using System.Net;
using Flurl.Http;
using GameWatch.Features.Auth;
using GameWatch.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using Quartz;
using Serilog;

namespace GameWatch.Features.IGameDatabase;

public class FetchGamesJob : IJob
{
    private readonly IOptions<TwitchOptions> _configuration;
    private readonly ITwitchAccessTokenService _twitchAccessTokenService;
    private readonly IDbContextFactory<GameWatchDbContext> _contextFactory;

    public FetchGamesJob(
        ITwitchAccessTokenService twitchAccessTokenService,
        IOptions<TwitchOptions> configuration,
        IDbContextFactory<GameWatchDbContext> contextFactory
    )
    {
        _twitchAccessTokenService = twitchAccessTokenService;
        _configuration = configuration;
        _contextFactory = contextFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        for (var i = 0; i < 20000; i += 500)
        {
            await SetupGames(i);
            await Task.Delay(500);
        }
    }

    private async Task SetupGames(int offset)
    {
        var token = await _twitchAccessTokenService.GetTwitchAccessTokenAsync(false);
        var clientId = _configuration.Value.ClientId;
        Console.WriteLine(token);
        /*
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

        const string url = "https://api.igdb.com/v4/games/";
        var body = $"fields *; limit 500; offset {offset};";
        var response = await authPolicy.ExecuteAsync(
            () =>
                url.WithHeader("Content-Type", "text/plain")
                    .WithHeader("Client-Id", clientId)
                    .WithOAuthBearerToken(token)
                    .PostAsync(new StringContent(body))
                    .ReceiveJson<Game[]>()
        );
        using (var dbContext = _contextFactory.CreateDbContext())
        {
            dbContext.Games.AddRange(response);
            await dbContext.SaveChangesAsync();
        }
        */
    }
}
