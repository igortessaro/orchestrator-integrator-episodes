using RabbitMQ.Client;

namespace Orchestrator.Integrator.Application.Rabbitmq
{
    public interface IRabbitmqConnectionFactory : IDisposable
    {
        IModel Connect();
    }
}