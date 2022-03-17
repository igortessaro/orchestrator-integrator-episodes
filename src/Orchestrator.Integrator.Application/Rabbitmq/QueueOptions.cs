namespace Orchestrator.Integrator.Application.Rabbitmq
{
    public sealed class QueueOptions
    {
        public string Name { get; set; } = string.Empty;
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public IReadOnlyCollection<string> RoutingKeys { get; set; }
    }
}
