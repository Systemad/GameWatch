using GameWatch.Features.IGameDatabase.Models;

namespace GameWatch.Features.Blog.Models;

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
    public string GameId { get; set; }
    public Game Game { get; set; }

    public List<Post> Posts { get; set; }
}
