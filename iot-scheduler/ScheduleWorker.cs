using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace iot_scheduler
{
    public class ScheduleWorker : BackgroundService
    {
        private readonly ILogger<ScheduleWorker> _logger;

        public ScheduleWorker(ILogger<ScheduleWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = new TimeSpan(DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes,
                    DateTime.Now.TimeOfDay.Seconds);

                Parallel.ForEach(ScheduleRepository.GetRecords().FindAll(x => x.ShouldRun(now)), scheduleObj =>
                {
                    _logger.LogInformation($"Schedule {scheduleObj.Id} running at: {DateTime.Now}");
                    Parallel.ForEach(scheduleObj.Devices, deviceObj => { WebClient.Get(deviceObj.ActionUrl); });
                });

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}