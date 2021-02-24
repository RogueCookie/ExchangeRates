using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using Scheduler.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Scheduler.MediatR.Models;

namespace Scheduler.MediatR.Command
{
    public class AddNewJob : CommandModel, IRequest
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