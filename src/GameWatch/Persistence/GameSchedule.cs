namespace GameWatch.Persistence;

public class GameSchedule
{
    public string Id { get; set; }
    public string GameDatabaseId { get; set; }
    public DateTime ReleaseDate { get; set; }
    public ICollection<string> UserIdLetter { get; set; } // Users who are subscribed find better name
}