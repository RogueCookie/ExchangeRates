using BTBConnector.Constants;
using BTBConnector.Interfaces;
using BTBConnector.Models;
using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace BTBConnector.Services
{
    public class ClientConnectorService : IClientConnectorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ClientConnectorService> _logger;

        public ClientConnectorService(IHttpClientFactory httpClientFactory, ILogger<ClientConnectorService> logger)
        {
            //из фактори забираем свободного клиента
            _httpClient = httpClientFactory.CreateClient(HttpClientConstants.Daily);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<List<DailyRates>> DownloadDataDailyAsync(DateTime date)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt?date={date:dd.MM.YYYY}");

                TextReader textReader = new StringReader(response);

                var dailyRequestData = await textReader.ReadLineAsync();

                _logger.LogInformation(dailyRequestData);

                var csvReader = new CsvReader(textReader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = "|"
                });

                return csvReader.GetRecords<DailyRates>().ToList();

            }
            catch (Exception exception)
            {
                _logger.LogError("BTBConnector Request error {Message}", exception.Message);
                return null;
            }
        }
    }
}