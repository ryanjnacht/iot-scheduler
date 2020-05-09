using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace iot_scheduler.Entities
{
    [BsonIgnoreExtraElements]
    public class Device
    {
        [JsonProperty("accessKey")] public string? AccessKey;
        [JsonProperty("action")] public string? Action;
        [JsonProperty("deviceId")] public string? DeviceId;

        [JsonIgnore]
        public string ActionUrl =>
            $"{AppConfiguration.IotApiUrl}/devices/{DeviceId}/{Action}?accessKey={AccessKey}";

        [JsonIgnore]
        public string DeviceUrl => $"{AppConfiguration.IotApiUrl}/devices/{DeviceId}?accessKey={AccessKey}";
    }
}