namespace GameWatch.Features.Auth;

public interface ITwitchAccessTokenService
{
    Task<string> GetTwitchAccessTokenAsync(bool unauthorized);
}
