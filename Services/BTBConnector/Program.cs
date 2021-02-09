using Autofac.Extensions.DependencyInjection;
using BTBConnector.Models;
using BTBConnector.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OzExchangeRates.Core;
using Serilog;
using System.Threading.Tasks;

namespace BTBConnector
{
    internal class Program
    {
        private static IConfiguration _configuration;

        static async Task Main(string[] args)
        {
            using var host = new ServiceHost(args);

             host.ConfigureServices((builderContext, services) =>
             {
                 _configuration = builderContext.Configuration;

                 services.Configure<RabbitSettings>(_configuration.GetSection("RabbitSettings"));
                 services.AddSingleton<RabbitService>();
                 
                 var logger = new LoggerConfiguration()
                     .Enrich.FromLogContext()
                     .WriteTo.Console()
                     .CreateLogger();

                 services.AddLogging(loggingBuilder =>
                     loggingBuilder.AddSerilog(logger));
             },
                 (services) =>
                 {
                     return new AutofacServiceProviderFactory((container) =>
                     {
                         container.Populate(services);
                     });
                 });

            await host.RunAsync((serviceProvider) =>
            {
               var eventBus = serviceProvider.GetRequiredService<RabbitService>();
                eventBus.Start();
            });
        }
    }
}
