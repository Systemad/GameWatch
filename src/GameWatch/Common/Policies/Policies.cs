using System.Net;
using Flurl.Http;
using Polly;
using Polly.Retry;
using Serilog;

namespace GameWatch.Common.Policies;

public static class Policies
{
    private static AsyncRetryPolicy<FlurlHttpException> UnAuthorizedRetryPolicy
    {
        get
        {
            return Policy
                .HandleResult<FlurlHttpException>(
                    r => r.StatusCode is (int)HttpStatusCode.Unauthorized
                )
                //.Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(
                    new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(5)
                    },
                    (delegateResult, retryCount) =>
                    {
                        Log.Information(
                            $"[App|Policy]: Retry delegate fired, attempt {retryCount}"
                        );
                    }
                );
        }
    }
}
