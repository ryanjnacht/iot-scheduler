using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iot_scheduler.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

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
            //todo: one-way schedules (no end)

            _logger.LogInformation("Worker started at: {time}", DateTime.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                //new schedules
                foreach (var scheduleObj in ScheduleRepository.GetRecords().FindAll(x => x.ShouldRun())
                    .Where(scheduleObj => StatusRepository.RunningSchedules.All(x => x.Id != scheduleObj.Id)))
                {
                    scheduleObj.Started = DateTime.Now;
                    _logger.LogInformation($"Schedule {scheduleObj.Id} started at: {scheduleObj.Started}");

                    foreach (var deviceObj in scheduleObj.Devices)
                        WebClient.Get(deviceObj.StartUrl);

                    if (scheduleObj.EndTime != null)
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