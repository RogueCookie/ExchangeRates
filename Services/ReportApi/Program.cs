using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ReportApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Allow to read parameters appsettings.json and the parameters were
        /// overwritten with parameters that are passed to the docker compose file
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory())
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            var env = hostingContext.HostingEnvironment;
                            // fluent configuration
                            config
                                .SetBasePath(env.ContentRootPath)
                                .AddJsonFile("appsettings.json", true, true)
                                .AddEnvironmentVariables();
                        });
                    webBuilder.UseStartup<Startup>().UseDefaultServiceProvider(options =>
                        options.ValidateScopes = false); 
                });
    }
}

