using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OzExchangeRates.Core.Enums;
using OzExchangeRates.Core.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;
using OzExchangeRates.Core.Models;

namespace BTBConnector.Services
{
    public class RabbitService
    {
        private readonly ILogger<RabbitService> _logger;
        private readonly RabbitSettings _settings;
        private readonly AddNewJobModel _regSettings;
        private const string routingKey = "connectorToLoader";

        public RabbitService(IOptions<RabbitSettings> options, IOptions<AddNewJobModel> registerSettings, ILogger<RabbitService> logger)
        {
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
            _regSettings = registerSettings.Value ?? throw new ArgumentNullException(nameof(options)); 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Start()
        {
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
                RegistrationInScheduler(channel);
                Publish(channel);
            }
            catch (BrokerUnreachableException ex)
            {
               _logger.LogError(ex, "RabbitService Client DeclareChannel() error in BTB connector");
            }
        }

        /// <summary>
        /// Self registration for current service in Scheduler if not exist
        /// </summary>
        private void RegistrationInScheduler(IModel channel)
        {
            var modelForRegistration = _regSettings;
            var message = JsonConvert.SerializeObject(modelForRegistration);
            var messageBytes = Encoding.UTF8.GetBytes(message);


            channel.ExchangeDeclare(exchange: Exchanges.Scheduler.ToString(), type: ExchangeType.Direct);
            channel.BasicPublish(
                exchange: Exchanges.Scheduler.ToString(),
                routingKey: RoutingKeys.AddNewJob.ToString(),
                body: messageBytes);
        }

        /// <summary>
        /// Prepare and send message to the exchange
        /// </summary>
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