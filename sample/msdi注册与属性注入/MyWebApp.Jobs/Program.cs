using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyWebApp.Jobs;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;

namespace MyWebApp
{
    public partial class Program
    {
        IConfiguration Configuration;
        IScheduler scheduler;

        public static Task Main(string[] args) => new Program().Run(args);

        public async Task Run(string[] args)
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            Globals.Configuration = Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            services.AddSingleton(Configuration);
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            Globals.DI = serviceProvider;

            try
            {
                await Run(serviceProvider, args);
            }
            catch (Exception ex)
            {
                //...
            }
            finally
            {
                if (scheduler != null) await scheduler.Shutdown(true);
                serviceProvider.Dispose();
            }
        }

        void ConfigureServices(IServiceCollection services)
        {
            JsonNetExtensions.SerializerSettings.Converters.Add(new CustomDateTimeConverter
            {
                ReaderConverter = new IsoDateTimeConverter { DateTimeFormat = null },
                WriterConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff" },
            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddLog4Net();
                loggingBuilder.SetMinimumLevel(LogLevel.Debug);
            });

            configure_external_Services(services);

            //...
            //services.AddScoped<SimpleJob>();
        }

        async Task Run(IServiceProvider serviceProvider, string[] args)
        {
            scheduler = await GetScheduler(serviceProvider);

            await scheduler.Start();
            Console.ReadLine(); //must block-wait
        }

        async Task<IScheduler> GetScheduler(IServiceProvider serviceProvider)
        {
            var props = new NameValueCollection();
            foreach (var cfg in Configuration.GetSection("qjob-scheduler").GetChildren())
            {
                props[cfg.Key] = cfg.Value;
            }

            var jobFactory = new DIJobFactory(serviceProvider);
            var schedulerFactory = new DISchedulerFactory(props, jobFactory);
            var sched = await schedulerFactory.GetScheduler();

            return sched;
        }

        async Task<IScheduler> default_GetScheduler()
        {
            var props = new NameValueCollection();
            foreach (var cfg in Configuration.GetSection("qjob-scheduler").GetChildren())
            {
                props[cfg.Key] = cfg.Value;
            }

            var schedulerFactory = new StdSchedulerFactory(props);
            var sched = await schedulerFactory.GetScheduler();

            return sched;
        }
    }
}
