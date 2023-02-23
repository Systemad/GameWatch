using GameWatch.Common.Game;

namespace GameWatch.Features.IGameDatabase;

public interface IGameDatabaseApi
{
    Task<Game[]> GetGamesAsync(string[] filters);
    Task<Game> GetGameAsync(string id);
    Task<Game[]> GetGamesByMonthAsync(int year, int month);
}
