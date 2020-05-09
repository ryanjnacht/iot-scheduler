#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using iot_scheduler.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace iot_scheduler.Controllers
{
    [DisableCors]
    [ApiController]
    [Route("schedules")]
    public class ScheduleController : ControllerBase
    {
        [HttpGet]
        public List<Schedule> GetSchedules()
        {
            return ScheduleRepository.GetRecords();
        }

        [HttpPost]
        public Schedule CreateSchedule([FromBody] JObject json)
        {
            var startTime = json["start_time"]?.ToString();
            if (string.IsNullOrEmpty(startTime))
                throw new Exception("start_time is required");

            int[]? days = null;
            if (json["days"] != null)
                try
                {
                    days = ((JArray) json["days"]!).Select(jv => (int) jv).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw new Exception($"could not parse days: {ex.Message}");
                }

            if (json["devices"] == null)
                throw new Exception("devices ia required but was not defined");

            var devices = json["devices"]?.Value<JArray>().ToObject<List<Device>>();

            if (devices == null || !devices.Any())
                throw new Exception("could not deserialize devices");

            var scheduleObj = new Schedule(startTime, days, devices);

            ScheduleRepository.Insert(scheduleObj);
            return ScheduleRepository.GetRecord(scheduleObj.Id);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            ScheduleRepository.DeleteRecord(id);
        }
    }
}