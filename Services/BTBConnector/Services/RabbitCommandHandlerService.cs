using System;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OzExchangeRates.Core.Enums;
using OzExchangeRates.Core.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BTBConnector.Services
{
    public class RabbitCommandHandlerService : BackgroundService
    { 
        private readonly RabbitSettings _options;
        private readonly IMediator _mediator;
        private readonly AddNewJobModel _registerSettings;
        private readonly ILogger<RabbitCommandHandlerService> _logger;
        private IConnection _connection;
        private IModel _channel;

        public RabbitCommandHandlerService(IOptions<RabbitSettings> options,
                                           IOptions<AddNewJobModel> registerSettings,
                                           ILogger<RabbitCommandHandlerService> logger)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _registerSettings = registerSettings.Value ?? throw new ArgumentNullException(nameof(registerSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            InitializeRabbitMQListener();
            return Task.CompletedTask;
        }


        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Through one queue, the Scheduler will send a message (command) which work must be executed 
        /// </summary>
        private void InitializeRabbitMQListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                UserName = _options.Login,
                Password = _options.Password,
                Port = _options.Port
            };

            _connection = factory.CreateConnection(clientProvidedName: "BTBConnector listener");
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(Exchanges.Scheduler.ToString(), ExchangeType.Direct);

            var queueName = "Execute.Job.Btb";
            _channel.QueueDeclare(queueName, exclusive: false, durable: true, autoDelete: false);
            _channel.BasicQos(0, 1, false);
            _channel.QueueBind(queueName, Exchanges.Scheduler.ToString(), _registerSettings.RoutingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, args) =>
            {
                var body = args.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                var commandModel = JsonConvert.DeserializeObject<AddNewJobModel>(message);
                ExecuteCommand(commandModel.JobName);
                _logger.LogInformation($"Consumer {queueName} with mes {message}");
            };
            _channel.BasicConsume(queueName, consumer: consumer, autoAck: false);
            _logger.LogInformation("BTB connector get command from scheduler");
        }

        private void ExecuteCommand(string command)
        {
            switch (command)
            {
                case "Download":
                    Download();
                    break;
                case "StoreDate":
                    Store();
                    break;
                default:
                    throw new Exception();
            }
        }

        private void Store()
        {
            Console.WriteLine("Store");
        }

        private void Download()
        {
            Console.WriteLine("Download");
        }
    }
}