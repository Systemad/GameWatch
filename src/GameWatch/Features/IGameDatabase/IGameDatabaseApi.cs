using GameWatch.Common.Models;

namespace GameWatch.Features.IGameDatabase;

public interface IGameDatabaseApi
{
    // fix T generic
    Task<Game[]> GetGamesAsync(string[] filters);
}
