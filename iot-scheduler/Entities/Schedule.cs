using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace iot_scheduler.Entities
{
    [BsonIgnoreExtraElements]
    public class Schedule : IEntity
    {
        [JsonProperty("days")] public int[]? Days;
        [JsonProperty("devices")] public List<Device> Devices;
        [JsonProperty("end_time")] public TimeSpan? EndTime;
        [JsonIgnore] [BsonIgnore] public DateTime Started;
        [JsonProperty("start_time")] public TimeSpan StartTime;

        public Schedule(string startTime, int? duration, int[]? days, List<Device> devices)
        {
            Id = Guid.NewGuid().ToString();
            StartTime = TimeSpan.Parse(startTime);
            Days = days;
            Devices = devices;

            if (duration != null)
                EndTime = StartTime.Add(new TimeSpan(0, 0, (int) duration));
        }

        [JsonIgnore]
        [BsonIgnore]
        public DateTime EndsAt => EndTime != null ? Started.Add(((TimeSpan) EndTime).Subtract(StartTime)) : default;

        [BsonElement("_id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        public bool ShouldRun()
        {
            if (Days != null && Days.Any())
                if (!Days.Contains((int) DateTime.Now.DayOfWeek))
                    return false;

            var now = new TimeSpan(DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes,
                DateTime.Now.TimeOfDay.Seconds);

            // one-time
            if (EndTime == null)
                return StartTime == now;

            // see if start comes before end
            if (StartTime < EndTime)
                return StartTime <= now && now <= EndTime;

            // start is after end, so do the inverse comparison
            return !(EndTime < now && now < StartTime);
        }
    }
}