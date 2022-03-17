namespace Orchestrator.Integrator.Application.Rabbitmq
{
    public sealed class Message<TData>
    {
        public string RoutingKey { get; set; }
        public string Exchange { get; set; }
        public string ConsumerTag { get; set; }
        public string CorrelationId { get; set; }
        public string MessageId { get; set; }
        public TData Body { get; set; }

        public Message(string routingKey, string exchange, string consumerTag, string correlationId, string messageId, TData body)
        {
            RoutingKey = routingKey;
            Exchange = exchange;
            ConsumerTag = consumerTag;
            CorrelationId = correlationId;
            MessageId = messageId;
            Body = body;
        }
    }
}
