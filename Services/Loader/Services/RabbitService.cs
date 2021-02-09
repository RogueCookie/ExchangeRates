using Loader.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Loader.Services
{
    public class RabbitService
    {
        private readonly ILogger<RabbitService> _logger;
        private readonly RabbitSettings _settings;

        public RabbitService(IOptions<RabbitSettings> options, ILogger<RabbitService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public void Start()
        {
            _logger.LogInformation($"hostyName = { _settings.HostName}, port = { _settings.Port}, login = { _settings.Login}. password = { _settings.Password}");
        }
    }
}