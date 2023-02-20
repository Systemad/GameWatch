namespace GameWatch.Features.Auth;

public class TwitchOptions
{
    public const string Twitch = "Twitch";

    public string ClientId { get; set; } = String.Empty;
    public string ClientSecret { get; set; } = String.Empty;
}
