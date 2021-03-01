using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using OzExchangeRates.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.MediatR.Command
{
    public class AddNewJob : AddNewJobModel, IRequest
    {

    }

    public class AddNewJobHandler : IRequestHandler<AddNewJob>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AddNewJobHandler> _logger;

        public AddNewJobHandler(IMediator mediator, ILogger<AddNewJobHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Add new job if not exist or update previous one
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Unit> Handle(AddNewJob request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"reccuring job was started");
            RecurringJob.AddOrUpdate(request.JobName, () => Send(request), request.CronScheduler);
            return Unit.Task;
        }

        public void Send(AddNewJob request)
        {
            _mediator.Send(new SendCommand()
            {
                Version = request.Version,
                Command = request.Command,
                RoutingKey = request.RoutingKey,
                JobName = request.JobName
            }).GetAwaiter().GetResult();
        }
    }
}