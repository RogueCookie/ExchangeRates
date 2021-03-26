using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OzExchangeRates.Core.Enums;
using OzExchangeRates.Core.Models;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.MediatR.Command
{
    public class SendCommand : AddNewJobModel, IRequest
    {
    }

    public class SendCommandHandler : IRequestHandler<SendCommand>
    {
        private readonly RabbitSettings _settingOptions;

        public SendCommandHandler(IOptions<RabbitSettings> settingOptions)
        {
            _settingOptions = settingOptions.Value;
        }

        /// <summary>
        /// Send command by mediator
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Unit> Handle(SendCommand request, CancellationToken cancellationToken) 
        {
            var message = JsonConvert.SerializeObject(request);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            var factory = new ConnectionFactory { HostName = _settingOptions.HostName, UserName = _settingOptions.Login, 
                Password = _settingOptions.Password, Port = _settingOptions.Port };
            
            using var connection = factory.CreateConnection(clientProvidedName: "Scheduler send command");
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: Exchanges.Scheduler.ToString(), type: ExchangeType.Direct);
            channel.BasicPublish(
                exchange: Exchanges.Scheduler.ToString(),
                routingKey: request.RoutingKey,
                body: messageBytes);

            channel.Close();
            connection.Close();

            return await Unit.Task;
        }
    }
}