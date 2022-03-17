using Hangfire.Server;

namespace Orchestrator.Integrator.Application.BackgroundJobs
{
    public interface IJob
    {
        string Name { get; }
        Task RunAsync(PerformContext? performContext, CancellationToken cancellationToken);
    }
}
