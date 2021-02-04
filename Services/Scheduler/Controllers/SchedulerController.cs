using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Scheduler.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [Produces("application/json", "application/xml")]
    [ApiController]
    public class SchedulerController : Controller
    {
        /// <summary>
        /// test comments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public ActionResult<string> Test()
        {
            return "Hello, Scheduler!";
        }
    }
}
