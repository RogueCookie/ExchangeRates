namespace OzExchangeRates.Core.Models
{
    /// <summary>
    /// Data which are used for register a new job in hangfire with particular settings for rabbitMq
    /// </summary>
    public class AddNewJobModel
    {
        /// <summary>
        /// Version of created job
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Command for execution
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Name of the job
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// Whether job marked as enabled (active) or not
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Routing by which this job  will be communicated with exchange
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// Cron settings (time period for execution)
        /// </summary>
        public string CronScheduler { get; set; }
    }
}