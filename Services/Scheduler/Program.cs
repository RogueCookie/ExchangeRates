using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Scheduler
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
                            // configure service settings  
                            config
                                // set the base path as the current execution path 
                                .SetBasePath(env.ContentRootPath)
                                // add particular json file
                                .AddJsonFile("appsettings.json", true, true)
                                //.AddJsonFile("jobs.json", true, true) //TODO
                                // reads the configuration value from the environment variable 
                                .AddEnvironmentVariables();
                        });
                    webBuilder.UseStartup<Startup>().UseDefaultServiceProvider(options =>
                        options.ValidateScopes = false);
                });
    }
}
