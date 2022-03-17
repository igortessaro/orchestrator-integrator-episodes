using Hangfire.Server;
using Orchestrator.Integrator.Application.RickAndMorty.Services;

namespace Orchestrator.Integrator.Application.RickAndMorty.Jobs
{
    public class SendEpisodesJob : ISendEpisodesJob
    {
        private readonly ILogger<SendEpisodesJob> _logger;
        private readonly ISendEpisodeService _createEpisodeService;

        public string Name => nameof(SendEpisodesJob);

        public SendEpisodesJob(ILogger<SendEpisodesJob> logger, ISendEpisodeService createEpisodeService)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._createEpisodeService = createEpisodeService ?? throw new ArgumentNullException(nameof(createEpisodeService));
        }

        public async Task RunAsync(PerformContext? performContext, CancellationToken cancellationToken)
        {
            using (this._logger.BeginScope("{@BackgroundJob}", performContext?.BackgroundJob))
            using (this._logger.BeginScope("{CorrelationId}", performContext?.BackgroundJob.Id))
            {
                this._logger.LogInformation("Starting the execution of job {jobName} with {@performContext}", this.Name, performContext);

                await this._createEpisodeService.Consume(cancellationToken);

                this._logger.LogInformation("Finish the execution of job {jobName} with {@performContext}", this.Name, performContext);
            }
        }
    }
}
