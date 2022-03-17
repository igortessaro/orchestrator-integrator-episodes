namespace Orchestrator.Integrator.Application.Rabbitmq
{
    public sealed class MessageFactory : IMessageFactory
    {
        public Message<TData> Create<TData>(string routingKey, string exchange, string consumerTag, string correlationId, string messageId, TData body)
        {
            return new Message<TData>(routingKey, exchange, consumerTag, correlationId, messageId, body);
        }
    }
}
