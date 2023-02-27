using GameWatch.Common.Game;

namespace GameWatch.Features.IGameDatabase;

public interface IGameDatabaseApi
{
    Task<Game[]> GetGamesByIdAsync(string[] ids);
    Task<Game> GetGameAsync(string id);
    Task<Game[]> GetGamesByMonthAsync(int year, int month);
}
