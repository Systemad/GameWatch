namespace GameWatch.Features.Wishlist.Models;
using GameWatchUser = User.Models.User;

public class WishlistItem
{
    public string GameId { get; set; }
    public string UserId { get; set; }
    public GameWatchUser User { get; set; }
}
