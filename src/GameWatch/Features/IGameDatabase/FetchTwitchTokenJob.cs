using Quartz;

namespace GameWatch.Features.IGameDatabase;

public class FetchTwitchTokenJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        return Task.FromResult(true);
    }
}
