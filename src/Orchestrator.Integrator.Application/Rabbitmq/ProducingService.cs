using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;

namespace Orchestrator.Integrator.Application.Rabbitmq
{
    public sealed class ProducingService : IProducingService
    {
        private readonly ILogger<ProducingService> _logger;
        private readonly IRabbitmqConnectionFactory _rabbitmqConnectionFactory;
        private readonly ExchangeOptions _exchangeOptions;

        public ProducingService(
            ILogger<ProducingService> logger,
            IOptionsSnapshot<ExchangeOptions> exchangeOptions,
            IRabbitmqConnectionFactory rabbitmqConnectionFactory)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._exchangeOptions = exchangeOptions.Value;
            this._rabbitmqConnectionFactory = rabbitmqConnectionFactory ?? throw new ArgumentNullException(nameof(rabbitmqConnectionFactory));
        }

        public Task SendAsync<T>(T @object, string exchangeName, string routingKey, string? correlationId) where T : class
        {
            this._logger.LogDebug("Starting the execution {method} with exchange {@exchangeName} and routingKey {@routingKey}", nameof(SendAsync), exchangeName, routingKey);
            string body = JsonConvert.SerializeObject(@object);
            var queueConfig = this._exchangeOptions.Queues.First(q => q.RoutingKeys.Contains(routingKey));

            using var channel = this._rabbitmqConnectionFactory.Connect();
            channel.QueueDeclare(queue: queueConfig.Name, durable: queueConfig.Durable, exclusive: queueConfig.Exclusive, autoDelete: queueConfig.AutoDelete, arguments: null);
            IBasicProperties props = channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.MessageId = Guid.NewGuid().ToString();
            channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: props, body: Encoding.UTF8.GetBytes(body));

            this._logger.LogInformation("Producing message {MessageId} with correlationId {CorrelationId} and body {@body}", props.MessageId, props.CorrelationId, @object);

            return Task.CompletedTask;
        }
    }
}
