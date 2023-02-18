using Quartz;

/*
 *  Although you could use the expires_in value to proactively get a new token before the token expires,
 * you’re discouraged from using this approach because tokens can become invalid for a number of reasons (see How do tokens become invalid?).
 * Instead, Twitch recommends that apps reactively respond to HTTP status code 401 Unauthorized.
 * TLDR:
 * -- Do not use a cron job to fetch token,
 * -- Fetch token once, put in in memeorycache.
 * -- use token, if its 401, get a new token
 */
namespace GameWatch.Features.IGameDatabase;

public class FetchTwitchTokenJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        return Task.FromResult(true);
    }
}
