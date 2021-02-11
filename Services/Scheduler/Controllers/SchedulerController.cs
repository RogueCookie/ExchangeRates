using System;
using System.Net;
using System.Threading;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Scheduler.Models;

namespace Scheduler.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [Produces("application/json", "application/xml")]
    [ApiController]
    public class SchedulerController : Controller
    {
        private readonly JobServiceOptions _options;
        private readonly ILogger<SchedulerController> _logger;

        public SchedulerController(IOptions<JobServiceOptions> options, ILogger<SchedulerController> logger)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
