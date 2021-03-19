using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OzExchangeRates.Core.Enums;
using OzExchangeRates.Core.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;

namespace BTBConnector.Services
{
    /// <summary>
    /// regis job in scheduler
    /// </summary>
    public class RegisterJobService
    {
        private readonly ILogger<RegisterJobService> _logger;
        private readonly RabbitSettings _settings;
        private readonly AddNewJobModel _regSettings;
        private const string routingKey = "connectorToLoader";

        public RegisterJobService(IOptions<RabbitSettings> options, IOptions<AddNewJobModel> registerSettings, ILogger<RegisterJobService> logger)
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
    }
}