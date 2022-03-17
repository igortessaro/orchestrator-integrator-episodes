namespace Orchestrator.Integrator.Application.Rabbitmq
{
    public interface IMessageFactory
    {
        Message<TData> Create<TData>(string routingKey, string exchange, string consumerTag, string correlationId, string messageId, TData body);
    }
}