using System.Collections.Generic;

namespace Scheduler.Models
{
    /// <summary>
    /// Describe fields from the job during reading data from configuration file
    /// </summary>
    public class JobOption
    {
        /// <summary>
        /// Unique name of job
        /// </summary>
        public string UniqueName { get; set; }

        /// <summary>
        /// Condition Is enabled for reading
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Cron schedule expressions (min, hours, day, month, day of week)
        /// </summary>
        public string CronSchedule { get; set; }

        /// <summary>
        /// Name of the action method (class)
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Additional parameters like EndPointUrl 
        /// </summary>
        public Dictionary<string, string> Options { get; set; }
    }
}