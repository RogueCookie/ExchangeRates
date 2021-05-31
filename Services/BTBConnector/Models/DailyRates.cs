namespace BTBConnector.Models
{
    public class DailyRates
    {
        /// <summary>
        /// Country of currency
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Type of currency
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Amount of currency
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Code of currency
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Current rate
        /// </summary>
        public decimal Rate { get; set; }
    }
}