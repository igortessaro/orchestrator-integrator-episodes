using Orchestrator.Integrator.Application.Rabbitmq;
using Orchestrator.Integrator.Application.RickAndMorty.DataTransferObjects;

namespace Orchestrator.Integrator.Application.RickAndMorty.Services
{
    public class ProcessEpisodeService : IProcessEpisodeService
    {
        private readonly IProducingService _producingService;

        public ProcessEpisodeService(IProducingService producingService)
        {
            this._producingService = producingService ?? throw new ArgumentNullException(nameof(ProcessEpisodeService));
        }

        public Task Process(EpisodeDto episode, string jobId)
        {
            if (episode is null)
            {
                throw new ArgumentNullException(nameof(episode));
            }

            return this._producingService.SendAsync(episode, "episodes.exchange", "rickandmorty.api.routing.key", jobId);
        }
    }
}
