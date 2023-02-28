namespace GameWatch.Features.Auth;

public class TwitchOptions
{
    public const string Twitch = "Twitch";
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    //public string AccessToken { get; set; } = string.Empty;
}
