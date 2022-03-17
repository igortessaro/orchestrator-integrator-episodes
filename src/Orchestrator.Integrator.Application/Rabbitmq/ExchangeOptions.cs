namespace Orchestrator.Integrator.Application.Rabbitmq
{
    public sealed class ExchangeOptions
    {
        public const string Position = "RabbitMqExchange";

        public string Type { get; set; } = string.Empty;
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public string DeadLetterExchange { get; set; } = string.Empty;
        public bool RequeueFailedMessages { get; set; }
        public IReadOnlyCollection<QueueOptions> Queues { get; set; }
    }
}
