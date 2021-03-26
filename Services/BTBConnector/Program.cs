using Autofac.Extensions.DependencyInjection;
using BTBConnector.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OzExchangeRates.Core;
using OzExchangeRates.Core.Models;
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
                 services.Configure<AddNewJobModel>(_configuration.GetSection("RegisterSettings"));
                 services.AddSingleton<RegisterJobService>();
                 services.AddHostedService<RabbitCommandHandlerService>();

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
            // execute ones when service starts
            await host.RunAsync((serviceProvider) =>
            {
               var eventBus = serviceProvider.GetRequiredService<RegisterJobService>();
                eventBus.Start();
            });
        }
    }
}
