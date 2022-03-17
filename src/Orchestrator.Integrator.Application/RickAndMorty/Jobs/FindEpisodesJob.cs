using Hangfire.Server;
using Orchestrator.Integrator.Application.RickAndMorty.Services;

namespace Orchestrator.Integrator.Application.RickAndMorty.Jobs
{
    public class FindEpisodesJob : IFindEpisodesJob
    {
        public ILogger<FindEpisodesJob> _logger { get; set; }
        private readonly IFindEpisodeService _episodeService;
        private readonly IProcessEpisodeService _processEpisodeService;

        public string Name => nameof(FindEpisodesJob);

        public FindEpisodesJob(ILogger<FindEpisodesJob> logger, IFindEpisodeService episodeService, IProcessEpisodeService processEpisodeService)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._episodeService = episodeService ?? throw new ArgumentNullException(nameof(episodeService));
            this._processEpisodeService = processEpisodeService ?? throw new ArgumentNullException(nameof(processEpisodeService));
        }

        public async Task RunAsync(PerformContext? performContext, CancellationToken cancellationToken)
        {
            using (this._logger.BeginScope("{@BackgroundJob}", performContext?.BackgroundJob))
            using (this._logger.BeginScope("{CorrelationId}", performContext?.BackgroundJob.Id))
            {
                this._logger.LogInformation("Starting the execution of job {jobName} with {@performContext}", this.Name, performContext);
                string? backgroundJobId = performContext?.BackgroundJob.Id;
                var episodes = await this._episodeService.GetAll();

                foreach (var episode in episodes)
                {
                    await this._processEpisodeService.Process(episode, backgroundJobId ?? "N/A");
                    this._logger.LogInformation("Processed the episode: {@episode}", episode);
                }

                this._logger.LogInformation("Finish the execution of job {jobName} with {@performContext}", this.Name, performContext);
            }
        }
    }
}
