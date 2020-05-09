using System;
using System.Collections.Generic;
using System.Linq;
using iot_scheduler.Extensions;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace iot_scheduler.Entities
{
    [BsonIgnoreExtraElements]
    public class Schedule : IEntity
    {
        [JsonProperty("days")] public int[]? Days;
        [JsonProperty("devices")] public List<Device> Devices;
        [JsonProperty("start_time")] public TimeSpan StartTime;

        public Schedule(string startTime, int[]? days, List<Device> devices)
        {
            Id = Guid.NewGuid().ToString();
            Days = days;
            Devices = devices;
            StartTime = TimeSpan.Parse(startTime).WithoutMilliseconds();
        }

        [BsonElement("_id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        public bool ShouldRun(TimeSpan? now = null)
        {
            now ??= DateTime.Now.TimeOfDay.WithoutMilliseconds();

            Console.WriteLine($"comparing {StartTime} to now {now}");

            if (Days != null && !Days.Contains((int) DateTime.Now.DayOfWeek))
                return false;

            return StartTime == now;
        }
    }
}