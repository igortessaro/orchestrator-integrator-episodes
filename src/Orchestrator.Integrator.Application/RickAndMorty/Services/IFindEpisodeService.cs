using Orchestrator.Integrator.Application.RickAndMorty.DataTransferObjects;

namespace Orchestrator.Integrator.Application.RickAndMorty.Services
{
    public interface IFindEpisodeService
    {
        Task<IReadOnlyCollection<EpisodeDto>> GetAll();
    }
}