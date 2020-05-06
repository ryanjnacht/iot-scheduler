using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iot_scheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                AppConfiguration.Load();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Startup failed: {ex.Message}");
                return;
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => { services.AddHostedService<ScheduleWorker>(); })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<WebApiStartup>(); });
        }
    }
}