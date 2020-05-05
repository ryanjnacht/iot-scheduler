using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace iot_scheduler.Entities
{
    public class Schedule : IEntity
    {
        [JsonProperty("days")] public int[]? Days;
        [JsonProperty("end_time")] public TimeSpan EndTime;
        [JsonProperty("start_time")] public TimeSpan StartTime;
        [JsonProperty("devices")] public List<Device> Devices;
        [JsonIgnore] public DateTime Started;
        [JsonIgnore] public DateTime EndsAt => Started.Add(EndTime.Subtract(StartTime));
        public Schedule(string startTime, int duration, int[]? days, List<Device> devices)
        {
            Id = Guid.NewGuid().ToString();
            StartTime = TimeSpan.Parse(startTime);
            EndTime = StartTime.Add(new TimeSpan(0, 0, duration));
            Days = days;
            Devices = devices;
        }

        [BsonElement("_id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        public bool ShouldRun()
        {
            if (Days != null && Days.Any())
            {
                if (!Days.Contains((int) DateTime.Now.DayOfWeek))
                    return false;
            }

            var now = DateTime.Now.TimeOfDay;

            // see if start comes before end
            if (StartTime < EndTime)
                return StartTime <= now && now <= EndTime;

            // start is after end, so do the inverse comparison
            return !(EndTime < now && now < StartTime);
        }
    }
}