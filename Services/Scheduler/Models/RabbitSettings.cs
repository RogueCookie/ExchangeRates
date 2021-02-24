namespace Scheduler.Models
{
    /// <summary>
    /// Describe the main settings for rabbit environment
    /// </summary>
    public class RabbitSettings
    {
        /// <summary>
        /// Name of the host
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Port number
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Login for connection(login) in rabbit on the server
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Password for login in rabbit on the server
        /// </summary>
        public string Password { get; set; }
    }
}