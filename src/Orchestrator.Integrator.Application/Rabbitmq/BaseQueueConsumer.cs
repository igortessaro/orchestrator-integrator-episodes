using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Orchestrator.Integrator.Application.Rabbitmq
{
    public abstract class BaseQueueConsumer<TData>
    {
        private readonly ILogger _logger;
        private readonly QueueOptions _queueOptions;
        private readonly IRabbitmqConnectionFactory _rabbitmqConnectionFactory;
        private readonly IMessageFactory _messageFactory;

        public BaseQueueConsumer(
            ILogger logger, 
            QueueOptions queueOptions,
            IRabbitmqConnectionFactory rabbitmqConnectionFactory,
            IMessageFactory messageFactory)
        {
            this._logger = logger;
            this._queueOptions = queueOptions;
            this._rabbitmqConnectionFactory = rabbitmqConnectionFactory ?? throw new ArgumentNullException(nameof(rabbitmqConnectionFactory));
            this._messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));
        }

        public async Task Consume(CancellationToken cancellationToken)
        {
            this._logger.LogDebug("Starting the execution {method}", nameof(Consume));

            using var channel = this._rabbitmqConnectionFactory.Connect();

            this._logger.LogDebug("Try connect in the queue {@queue}", this._queueOptions);

            channel.QueueDeclare(queue: this._queueOptions.Name,
                                 durable: this._queueOptions.Durable,
                                 exclusive: this._queueOptions.Exclusive,
                                 autoDelete: this._queueOptions.AutoDelete,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
            {
                this._logger.LogDebug("Running consumer.Received with {@sender} and {@basicDeliverEventArgs}", sender, e);
                var message = this._messageFactory.Create(e.RoutingKey, e.Exchange, e.ConsumerTag, e.BasicProperties.CorrelationId, e.BasicProperties.MessageId, e.Body);
                this._logger.LogDebug("Received message {@message}", message);

                this.Received(message);
            };

            channel.BasicConsume(queue: this._queueOptions.Name, autoAck: true, consumer: consumer);

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug($"Worker {nameof(Consume)} running in: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                await Task.Delay(10000, cancellationToken);
            }
        }

        protected abstract Task Received(Message<ReadOnlyMemory<byte>> message);
    }
}
