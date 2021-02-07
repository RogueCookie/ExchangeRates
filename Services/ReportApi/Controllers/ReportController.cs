using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ReportApi.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [Produces("application/json", "application/xml")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        /// <summary>
        /// test
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public ActionResult<string> Test()
        {
            return "Hello, World!";
        }
    }
}
