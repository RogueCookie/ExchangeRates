using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using Scheduler.Services;

namespace Scheduler.MediatR.Command
{
    public class AddNewJob : IRequest
    {

    }

    public class AddNewJobHandler : IRequestHandler<AddNewJob>
    {
        private readonly RabbitPublishService _rabbitPublishService;
        private readonly ILogger<AddNewJobHandler> _logger;

        public AddNewJobHandler(RabbitPublishService rabbitPublishService, ILogger<AddNewJobHandler> logger)
        {
            _rabbitPublishService = rabbitPublishService ?? throw new ArgumentNullException(nameof(rabbitPublishService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<Unit> Handle(AddNewJob request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"reccuring job was started");
            RecurringJob.AddOrUpdate(() => _rabbitPublishService., job.CronSchedule);
        }
    }
}