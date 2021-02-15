using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Scheduler.Models;
using System;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Scheduler.Services
{
    public class RabbitPublishService
    {
        private readonly ILogger<RabbitPublishService> _logger;
        private readonly RabbitSettings _settings;
        private const string routingKey = "connectorToScheduler";

        public RabbitPublishService(IOptions<RabbitSettings> options, ILogger<RabbitPublishService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Setup connection to RabbitMQ server
        /// </summary>
        private void DeclareChannel()
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
                //Consume(channel);
            }
            catch (BrokerUnreachableException ex)
            {
                _logger.LogError($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogError($"{ex.Data.Keys}");
                _logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Read message from the queue 
        /// </summary>
        private static void Consume(IModel channel)
        {
            if (channel == null) throw new ArgumentNullException(nameof(channel));

            channel.ExchangeDeclare(Exchanges.Scheduler.ToString(), ExchangeType.Direct, true);

            var queues = channel.QueueDeclare();
            channel.QueueBind(queues.QueueName, Exchanges.Scheduler.ToString(), routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, args) =>
            {
                var body = args.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"We got message at {DateTime.Now}  with txt {message}");
            };
            channel.BasicConsume(queues.QueueName, true, consumer);
            Console.WriteLine("Press me, please");
            Console.ReadLine();
        }
    }
}