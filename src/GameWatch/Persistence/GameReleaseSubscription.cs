using GameWatch.Features.IGameDatabase.Models;

namespace GameWatch.Persistence;

public class GameReleaseSubscription
{
    public string Id { get; set; }
    public string GameId { get; set; }
    public Game Game { get; set; }

    public List<string> SubscribedUserIds { get; set; }
}
