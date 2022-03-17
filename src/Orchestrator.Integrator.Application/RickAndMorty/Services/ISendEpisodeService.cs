using Orchestrator.Integrator.Application.RickAndMorty.DataTransferObjects;

namespace Orchestrator.Integrator.Application.RickAndMorty.Services
{
    public interface ISendEpisodeService
    {
        Task Consume(CancellationToken cancellationToken);
    }
}