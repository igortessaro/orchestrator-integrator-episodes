namespace Orchestrator.Integrator.Application.Rabbitmq
{
    public sealed class RabbitmqOptions
    {
        public const string Position = "RabbitMq";

        public string HostName { get; set; } = string.Empty;
        public short Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
