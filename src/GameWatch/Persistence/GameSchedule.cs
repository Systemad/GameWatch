using GameWatch.Features.User.Models;

namespace GameWatch.Persistence;

public class GameSchedule
{
    public string Id { get; set; }
    public string GameDatabaseId { get; set; }
    public DateTime ReleaseDate { get; set; }
    public List<string> SubscribedUserIds { get; set; } // Users who are subscribed find better name
}
