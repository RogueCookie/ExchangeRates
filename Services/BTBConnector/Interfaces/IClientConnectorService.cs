using System;
using OzExchangeRates.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BTBConnector.Models;

namespace BTBConnector.Interfaces
{
    public interface IClientConnectorService
    {
        /// <summary>
        /// Download currency rates from particular source on current date
        /// </summary>
        /// <returns>List of clientModels (data serialized from source)</returns>
        Task<List<DailyRates>> DownloadDataDailyAsync(DateTime date);
    }
}