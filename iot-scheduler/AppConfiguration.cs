using System;
using MongoDB.Driver;

namespace iot_scheduler
{
    public static class AppConfiguration
    {
        public static int WebClientTimeout = 2000;
        public static string? MongoDatabase = "iot-scheduler";
        private static string? _mongoHost;
        private static int _mongoPort = 27017;
        public static string? IotApiUrl;

        public static MongoUrl MongoDbUrl =>
            new MongoUrlBuilder
            {
                ApplicationName = $"{MongoDatabase}_app",
                Server = new MongoServerAddress(_mongoHost, _mongoPort)
            }.ToMongoUrl();

        public static void Load()
        {
            //required
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGO_HOST")))
                throw new Exception("MONGO_HOST is not defined");

            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IOT_API")))
                throw new Exception("IOT_API is not defined");

            //optional
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGO_DB")))
                MongoDatabase = Environment.GetEnvironmentVariable("MONGO_DB");

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGO_PORT")))
                int.TryParse(Environment.GetEnvironmentVariable("MONGO_PORT"), out _mongoPort);

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TIMEOUT")))
                int.TryParse(Environment.GetEnvironmentVariable("TIMEOUT"), out WebClientTimeout);

            //load
            _mongoHost = Environment.GetEnvironmentVariable("MONGO_HOST");
            IotApiUrl = Environment.GetEnvironmentVariable("IOT_API");

            Console.WriteLine($"[Configuration] Configured to use mongo @ {MongoDbUrl}");
            Console.WriteLine($"[Configuration] Configured to use IOT-API @ {IotApiUrl}");
        }
    }
}