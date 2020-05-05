using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace iot_scheduler.Controllers
{
    [DisableCors]
    [ApiController]
    [Route("status")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public JObject GetStatus()
        {
            var responseObj = new JObject();
            var statusesObj = new JArray();

            foreach (var scheduleObj in StatusRepository.RunningSchedules)
            {
                var statusObj = new JObject
                {
                    {"scheduleId", scheduleObj.Id},
                    {"startedAt", scheduleObj.Started.ToString("G")},
                    {"endsAt", scheduleObj.EndsAt.ToString("G")}
                };
                statusesObj.Add(statusObj);
            }

            responseObj.Add("status", statusesObj);
            return responseObj;
        }
    }
}