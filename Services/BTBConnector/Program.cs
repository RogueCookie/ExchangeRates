using Autofac.Extensions.DependencyInjection;
using BTBConnector.Constants;
using BTBConnector.Interfaces;
using BTBConnector.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OzExchangeRates.Core;
using OzExchangeRates.Core.Models;
using Polly;
using Serilog;
using System;
using System.Net.Http;
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
                 services.AddTransient<IClientConnectorService, ClientConnectorService>();
                 
                 services.AddHttpClient(HttpClientConstants.Daily, client =>
                 {
                     client.BaseAddress = new Uri("https://www.cnb.cz");
                 });
                 //.AddPolicyHandler(GetRetryPolicy());

                 //services.AddHttpClient("yearly", client =>
                 //{
                 //    client.BaseAddress =
                 //        new Uri(
                 //            "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year=2018"); //TODO
                 //    client.DefaultRequestHeaders.Add("YearlyHeader", Guid.NewGuid().ToString());
                 //});
                 //.AddPolicyHandler(GetRetryPolicy());

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

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()//TODO
        {
            throw new NotImplementedException();
        }
    }
}
