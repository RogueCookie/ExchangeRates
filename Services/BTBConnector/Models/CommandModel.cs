namespace BTBConnector.Models
{
    public class CommandModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// TODO
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RoutingKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CronScheduler { get; set; }
    }
}