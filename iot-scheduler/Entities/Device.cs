using Newtonsoft.Json;

namespace iot_scheduler.Entities
{
    public class Device
    {
        [JsonProperty("deviceId")] public string? DeviceId;

        [JsonProperty("accessKey")] public string? AccessKey;

        [JsonProperty("start_action")] public string? StartAction;

        [JsonProperty("end_action")] public string? EndAction;

        [JsonIgnore]
        public string StartUrl => $"{AppConfiguration.IotApiUrl}/devices/{DeviceId}/{StartAction}?accessKey={AccessKey}";

        [JsonIgnore]
        public string EndUrl => $"{AppConfiguration.IotApiUrl}/devices/{DeviceId}/{EndAction}?accessKey={AccessKey}";

        [JsonIgnore] public string DeviceUrl => $"{AppConfiguration.IotApiUrl}/devices/{DeviceId}?accessKey={AccessKey}";
    }
}