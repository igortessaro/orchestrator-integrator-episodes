using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orchestrator.Integrator.Application.Rabbitmq;
using Orchestrator.Integrator.Application.RickAndMorty.DataTransferObjects;
using System.Text;

namespace Orchestrator.Integrator.Application.RickAndMorty.Services
{
    public class SendEpisodeService : BaseQueueConsumer<EpisodeDto>, ISendEpisodeService
    {
        private readonly ILogger<SendEpisodeService> _logger;

        public SendEpisodeService(
            ILogger<SendEpisodeService> logger,
            IRabbitmqConnectionFactory rabbitmqConnectionFactory,
            IMessageFactory messageFactory,
            IOptionsSnapshot<ExchangeOptions> exchangeOptions)
            : base(logger, exchangeOptions.Value.Queues.First(), rabbitmqConnectionFactory, messageFactory)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task Received(Message<ReadOnlyMemory<byte>> message)
        {
            var receivedMessage = Encoding.UTF8.GetString(message.Body.ToArray());
            var episode = JsonConvert.DeserializeObject<EpisodeDto>(receivedMessage);
            this._logger.LogInformation("Sended episode {@episode} with messageId {MessageId} and correlationId {CorrelationId}", episode, message.MessageId, message.CorrelationId);
            return Task.CompletedTask;
        }
    }
}
