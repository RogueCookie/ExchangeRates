using OzExchangeRates.Core.Enums;
using System;
using System.Collections.Generic;

namespace OzExchangeRates.Core.Models
{
    /// <summary>
    /// Allow to serialize/deserialize data when we get them from the clients (BtbConnector)
    /// </summary>
    public class ConnectorClientModel
    {
        /// <summary>
        /// Date and rates when particular records has this rates of currency
        /// </summary>
        public Dictionary<DateTime, decimal> Rates { get; set; }

        /// <summary>
        /// Type of currency
        /// </summary>
        public TypeOfCurrency TypeOfCurrency { get; set; }

        /// <summary>
        /// Name of source from where downloaded current rates
        /// </summary>
        public string SourceName { get; set; }
    }
}