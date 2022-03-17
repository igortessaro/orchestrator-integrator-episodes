using Orchestrator.Integrator.Application.RickAndMorty.DataTransferObjects;

namespace Orchestrator.Integrator.Application.RickAndMorty.Services
{
    public interface IProcessEpisodeService
    {
        Task Process(EpisodeDto episode, string jobId);
    }
}