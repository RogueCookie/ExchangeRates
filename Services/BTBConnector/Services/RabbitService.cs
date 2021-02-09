using BTBConnector.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace BTBConnector.Services
{
    public class RabbitService
    {
        private readonly ILogger<RabbitService> _logger;
        private readonly RabbitSettings _settings;
        private const string routingKey = "commctorToLoader";

        public RabbitService(IOptions<RabbitSettings> options, ILogger<RabbitService> logger)
        {
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Start()
        {
            //_logger.LogInformation($"hostyName = {_settings.HostName}, port = {_settings.Port}, login = {_settings.Login}. password = {_settings.Password}");
            DeclareChannel();
        }

        /// <summary>
        /// Setup connection to RabbitMQ server
        /// </summary>
        public void DeclareChannel()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _settings.HostName,
                    UserName = _settings.Login,
                    Password = _settings.Password,
                    Port = _settings.Port
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                Publish(channel);
            }
            catch (BrokerUnreachableException ex)
            {
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Data.Keys);
                Console.WriteLine(ex.ToString());
            }
        }

        private static void Publish(IModel channel)
        {
            if (channel == null) throw new ArgumentNullException(nameof(channel));

            channel.ExchangeDeclare(Exchanges.Loader.ToString(), ExchangeType.Direct, true);

            while (true)
            {
                var message = "hello, from BTB";
                var newBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(Exchanges.Loader.ToString(), routingKey, null, newBody);
            }
        }
    }
}