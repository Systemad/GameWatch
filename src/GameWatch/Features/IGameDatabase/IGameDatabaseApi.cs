using GameWatch.Common.Models;

namespace GameWatch.Features.IGameDatabase;

public interface IGameDatabaseApi
{
    Task<Game[]> GetGamesAsync(string[] filters);
    Task<Game> GetGameAsync(string id);
}
