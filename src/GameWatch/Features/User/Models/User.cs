using GameWatch.Features.Wishlist.Models;

namespace GameWatch.Features.User.Models;

public class User
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    public List<WishlistItem> WishlistItems { get; set; }
}
