using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iot_scheduler.Entities;
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
            StatusRepository.RunningSchedules = new List<Schedule>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                //new schedules
                foreach (var scheduleObj in DataAccess<Schedule>.GetRecords().FindAll(x => x.ShouldRun())
                    .Where(scheduleObj => StatusRepository.RunningSchedules.All(x => x.Id != scheduleObj.Id)))
                {
                    foreach (var deviceObj in scheduleObj.Devices) WebClient.Get(deviceObj.StartUrl);

                    scheduleObj.Started = DateTime.Now;

                    StatusRepository.RunningSchedules.Add(scheduleObj);
                }

                //maintain schedules
                foreach (var scheduleObj in StatusRepository.RunningSchedules.ToList().Where(scheduleObj =>
                    !scheduleObj.ShouldRun()))
                {
                    foreach (var deviceObj in scheduleObj.Devices) WebClient.Get(deviceObj.EndUrl);

                    StatusRepository.RunningSchedules.Remove(scheduleObj);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}