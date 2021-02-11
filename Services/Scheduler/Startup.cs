using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Hangfire.Dashboard;
using Scheduler.Models;

namespace Scheduler
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// DI for startup
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Exchange scheduler api",
                    Description = "API for work with microservices architecture, part of Scheduler api",
                    Contact = new OpenApiContact()
                    {
                        Email = "v9527906422@gmail.com",
                        Name = "Valeriia Vaganova",
                        Url = new Uri("https://www.facebook.com/valeriia.vaganova.9/")
                    }
                });

                // path from which project to read xml documentation
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                c.IncludeXmlComments(xmlCommentsFullPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
            
            services.AddHangfire(configuration => configuration
                //.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                //.UseSimpleAssemblyNameTypeSerializer()
                //.UseDefaultTypeSerializer()
                .UsePostgreSqlStorage(_configuration.GetConnectionString("SchedulerDbConnection")));

            services.Configure<JobServiceOptions>(_configuration.GetSection("JobService"));
            services.AddHangfireServer();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger/*, IBackgroundJobClient backgroundJobClient*/)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Specify the Swagger endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"v1/swagger.json", $"Exchange rates scheduler v1");
                c.DisplayRequestDuration();
            });

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "text/html";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var exceptionHandlerFeature =
                        context.Features.Get<IExceptionHandlerFeature>();
                    logger.LogError(new EventId(), exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);

                    await context.Response.WriteAsync("Something wrong");
                });
            });

            app.UseHangfireServer();
           
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                IgnoreAntiforgeryToken = true,
                Authorization = new List<IDashboardAuthorizationFilter>(){}
            });

            //backgroundJobClient.Enqueue(() => Console.WriteLine("Hello, how are you"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
