using System.Collections.Generic;

namespace Scheduler.Models
{
    /// <summary>
    /// Represent the list of all jobs
    /// </summary>
    public class JobServiceOptions
    {
        public IEnumerable<JobOption> Jobs { get; set; }
    }
}