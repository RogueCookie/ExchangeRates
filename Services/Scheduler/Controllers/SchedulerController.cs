using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Scheduler.Models;
using System;
using System.Net;
using System.Threading;

namespace Scheduler.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [Produces("application/json", "application/xml")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly JobServiceOptions _options;
        private readonly ILogger<SchedulerController> _logger;

        public SchedulerController(IOptions<JobServiceOptions> options, ILogger<SchedulerController> logger)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public ActionResult ExecuteJobAsync(/*JobOption jobOption*/)
        {
            foreach (var job in _options.Jobs)
            {
                _logger.LogInformation($"reccuring job was started");
                RecurringJob.AddOrUpdate(() => RunInBackground(), job.CronSchedule);
            }
            //RecurringJob.AddOrUpdate(() => RunInBackground(), Cron.Minutely);

            return Ok();
        }
        
        /// <summary>
        /// test comments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public ActionResult<string> Test()
        {
            BackgroundJob.Enqueue( () => RunInBackground());
            return $"Hello, Scheduler! {Thread.CurrentThread.Name}";
        }

        public static void RunInBackground()
        {
            Console.WriteLine($"good job,val from {Thread.CurrentThread.Name}");
        }
    }
}
