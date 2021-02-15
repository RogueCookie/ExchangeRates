using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BTBConnector.Enums;
using BTBConnector.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BTBConnector.Services
{
    /// <summary>
    /// Сервис для работы с очередями
    /// </summary>
    public class RabbitMQService : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RabbitMQService> _logger;
        private readonly string _hostname;
        private readonly int _port;
        private IConnection _connection;
        private IModel _channel;
        private readonly string _username;
        private readonly string _password;

        public RabbitMQService(IOptions<RabbitSettings> options,
            IMediator mediator, ILogger<RabbitMQService> logger)
        {
            _mediator = mediator;
            _logger = logger;
            _hostname = options.Value.HostName;
            _port = options.Value.Port;
            _username = options.Value.Login;
            _password = options.Value.Password;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            InitializeRabbitMQListener();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Get the message from scheduler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
            try
            {
                _logger.LogInformation($"Scheduler consume {e.RoutingKey} Received {message}");
                var commandModel = JsonConvert.DeserializeObject<CommandModel>(message);


            }
            catch (Exception exception)
            {
                response = $"Error parse JSON {message}";
                _logger.LogError(response, exception);
            }
            finally
            {
                if (props.ReplyTo != null)
                {
                    var replyProps = _channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    _channel.BasicPublish("", props.ReplyTo,
                        replyProps, responseBytes);
                }

                _channel?.BasicAck(e.DeliveryTag, false);
            }
        }

        // ReSharper disable once InconsistentNaming
        private void InitializeRabbitMQListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password,
                Port = _port
            };

            _connection = factory.CreateConnection(clientProvidedName: "Scheduler listener");
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(Exchanges.Scheduler.ToString(), ExchangeType.Direct);

            var queueName = "Register.New.Job";
            _channel.QueueDeclare(queueName, exclusive: false, durable: true, autoDelete: false);
            _channel.BasicQos(0, 1, false);
            _channel.QueueBind(queueName, Exchanges.Scheduler.ToString(), RoutingKey.AddNewJob.ToString());

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnReceived;
            _channel.BasicConsume(queueName, consumer: consumer, autoAck: false);
            _logger.LogInformation("Scheduler registrations is Ok");
        }
    }
}