
namespace Orchestrator.Integrator.Application.Rabbitmq
{
    public interface IProducingService
    {
        Task SendAsync<T>(T @object, string exchangeName, string routingKey, string? correlationId) where T : class;
    }
}