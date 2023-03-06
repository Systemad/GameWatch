using GameWatch.Features.IGameDatabase.Models;

namespace GameWatch.Features.IGameDatabase;

public interface IGameDatabaseApi
{
    Task<T> GenericFetch<T>(string url, string body);
}
