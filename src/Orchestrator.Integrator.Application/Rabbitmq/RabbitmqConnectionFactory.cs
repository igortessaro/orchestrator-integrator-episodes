using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Orchestrator.Integrator.Application.Rabbitmq
{
    public sealed class RabbitmqConnectionFactory : IRabbitmqConnectionFactory
    {
        private bool _disposedValue;
        private readonly ILogger<RabbitmqConnectionFactory> _logger;
        private readonly RabbitmqOptions _rabbitmqOptions;
        private IConnection? _connection;
        private IModel? _model;

        public RabbitmqConnectionFactory(ILogger<RabbitmqConnectionFactory> logger, IOptionsSnapshot<RabbitmqOptions> rabbitmqOptions)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._rabbitmqOptions = rabbitmqOptions.Value;
        }

        public IModel Connect()
        {
            var factory = new ConnectionFactory()
            {
                HostName = this._rabbitmqOptions.HostName,
                Port = this._rabbitmqOptions.Port,
                UserName = this._rabbitmqOptions.UserName,
                Password = this._rabbitmqOptions.Password
            };

            this._connection = factory.CreateConnection();
            this._model = this._connection.CreateModel();

            return this._model;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                this._model?.Dispose();
                this._connection?.Dispose();
            }

            _disposedValue = true;
        }
    }
}
