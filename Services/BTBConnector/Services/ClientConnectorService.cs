using BTBConnector.Interfaces;
using OzExchangeRates.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BTBConnector.Services
{
    public class ClientConnectorService : IClientConnectorService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ClientConnectorService> _logger;

        public ClientConnectorService(
            IHttpClientFactory httpClientFactory, 
            ILogger<ClientConnectorService> logger
            )
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<List<ConnectorClientModel>> DownloadDataDailyAsync()
        {
            var currentDate = DateTime.Today.Date;
            try
            {
                var client = _httpClientFactory.CreateClient("daily");
                //client.GetStringAsync($"/daily.txt?date=27.07.2018");
                var response = await client.GetAsync($"/daily.txt?date=27.07.2018");
                //var currencyList = JsonSerializer.Deserialize<List<ConnectorClientModel>>(client);
                if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
                {
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Cannot connect to the Btb client and read data from url for daily");
                throw;
            }
            
            throw new NotImplementedException();
        }
    }
}