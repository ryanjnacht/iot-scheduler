using System.Collections.Generic;
using System.Linq;
using iot_scheduler.Entities;

namespace iot_scheduler
{
    public static class ScheduleRepository
    {
        private static List<Schedule>? _cache;

        private static bool UseCache => AppConfiguration.UseCache;

        public static void BuildCache()
        {
            _cache = DataAccess<Schedule>.GetRecords();
        }

        public static void Insert(Schedule scheduleObj)
        {
            DataAccess<Schedule>.Insert(scheduleObj);
            if (UseCache) _cache?.Add(scheduleObj);
        }

        public static Schedule GetRecord(string id)
        {
            return UseCache ? _cache.FirstOrDefault(x => x.Id == id) : DataAccess<Schedule>.GetRecord(id);
        }

        public static List<Schedule> GetRecords()
        {
            return UseCache ? _cache! : DataAccess<Schedule>.GetRecords();
        }

        public static void DeleteRecord(string id)
        {
            DataAccess<Schedule>.DeleteRecord(id);
            if (UseCache) _cache?.RemoveAll(x => x.Id == id);
        }
    }
}