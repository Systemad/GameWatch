using GameWatch.Features.Auth;
using GameWatch.Features.IGameDatabase.Models;
using GameWatch.Persistence;
using Microsoft.Extensions.Options;
using Quartz;

namespace GameWatch.Features.IGameDatabase;

public class FetchGamesJob : IJob
{
    private readonly IOptions<TwitchOptions> _configuration;
    private readonly ITwitchAccessTokenService _twitchAccessTokenService;
    private readonly IGameDatabaseApi _gameDatabaseApi;

    private const int maxItemsPerIteration = 500;

    public FetchGamesJob(
        ITwitchAccessTokenService twitchAccessTokenService,
        IOptions<TwitchOptions> configuration,
        IGameDatabaseApi gameDatabaseApi
    )
    {
        _twitchAccessTokenService = twitchAccessTokenService;
        _configuration = configuration;
        _gameDatabaseApi = gameDatabaseApi;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var countResponse = await _gameDatabaseApi.GenericFetch<CountResponse>(
            "https://api.igdb.com/v4/games/count",
            "*;"
        );

        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;
        long offset = 0;
        long numIterations = 0;

        for (long i = 0; i < iterations; i++)
        {
            long itemsToTake = Math.Min(
                maxItemsPerIteration,
                totalItems - i * maxItemsPerIteration
            );
            offset += itemsToTake;
            numIterations++;
            await SetupGames(itemsToTake, offset);
            await Task.Delay(500);
        }
    }

    private async Task SetupGames(long itemsToTake, long offset)
    {
        const string url = "https://api.igdb.com/v4/games";
        var body =
            $"fields age_ratings.rating,age_ratings.category,artworks.image_id,category,cover.image_id,screenshots.image_id,release_dates.*,release_dates.platform.name,game_engines.name,game_modes.name,genres.name,involved_companies.*,involved_companies.company.name,first_release_date,keywords.name,multiplayer_modes,name,platforms.name,player_perspectives.name,rating,release_dates.*,similar_games.name,similar_games.cover.image_id,slug,status,storyline,summary,themes.name,url,version_title,websites.*; limit {itemsToTake}; offset {offset};";

        var response = await _gameDatabaseApi.GenericFetch<Game[]>(url, body);
        using (var dbContext = new GameContext())
        {
            dbContext.Games.AddRange(response);
            dbContext.SaveChanges();
        }
    }
}
