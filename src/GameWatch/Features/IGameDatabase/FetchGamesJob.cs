using System.Net;
using Flurl.Http;
using GameWatch.Features.Auth;
using GameWatch.Features.IGameDatabase.Models;
using GameWatch.Persistence;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Options;
using Polly;
using Quartz;
using Serilog;

namespace GameWatch.Features.IGameDatabase;

public class FetchGamesJob : IJob
{
    private readonly IOptions<TwitchOptions> _configuration;
    private readonly ITwitchAccessTokenService _twitchAccessTokenService;

    public FetchGamesJob(
        ITwitchAccessTokenService twitchAccessTokenService,
        IOptions<TwitchOptions> configuration
    )
    {
        _twitchAccessTokenService = twitchAccessTokenService;
        _configuration = configuration;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // https://api.igdb.com/v4/games/count

        //for (var i = 0; i < 20000; i += 500 + 1)
        //{
        await SetupGames(5001);
        //await Task.Delay(500);
        //}
    }

    private async Task SetupGames(int offset)
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

        const string url = "https://api.igdb.com/v4/games";
        //var body = $"fields *; limit 500; offset {offset};";
        var body =
            $"fields age_ratings.rating,age_ratings.category,artworks.image_id,category,cover.image_id,screenshots.image_id,release_dates.*,release_dates.platform.name,game_engines.name,game_modes.name,genres.name,involved_companies.*,involved_companies.company.name,first_release_date,keywords.name,multiplayer_modes,name,platforms.name,player_perspectives.name,rating,release_dates.*,similar_games.name,similar_games.cover.image_id,slug,status,storyline,summary,themes.name,url,version_title,websites.*; where id = 1942;";

        var response = await authPolicy.ExecuteAsync(
            () =>
                url.WithHeader("Content-Type", "text/plain")
                    .WithHeader("Client-Id", clientId)
                    .WithOAuthBearerToken(token)
                    .PostAsync(new StringContent(body))
                    .ReceiveJson<Game[]>()
        );

        var hey = "";
        /*
                var db = new DataConnection(
                    new DataOptions().UsePostgreSQL("",
                        @"Host=localhost;Port=5433;Database=gamesdb;Username=postgres;Password=Compaq2009"
                    )
                );
                */
        /*
        using (var dbContext = new GameContext())
        {
            dbContext.Games.AddRange(deserialied);
            dbContext.SaveChanges();
        }
        */
    }
}
