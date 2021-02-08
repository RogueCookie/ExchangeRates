using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OzExchangeRates.Core;
using Serilog;
using System;
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

                 var connectionElasticLogger = _configuration.GetConnectionString("ElasticLogger");

                 var connection = _configuration.GetConnectionString("Updater");
                 var workDb = _configuration.GetValue<string>("ConnectionString");
                 var schema = _configuration.GetValue<string>("MySchemaName");
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
                var baseInit = _configuration.GetValue<bool?>("BaseInit");
                Log.Information($"Base Init - {baseInit}");
                if (baseInit.HasValue && baseInit.Value)
                {
                    try
                    {
                        //var dbInit = serviceProvider.GetRequiredService<DbInitializator>();
                        //dbInit.Initialize().GetAwaiter().GetResult();
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception.Message);
                    }
                }
                //var eventBus = serviceProvider.GetRequiredService<RabbitMQClient>();
                //eventBus.Start();
            });
        }
    }
}
