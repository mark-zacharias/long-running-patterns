using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Azure.Functions
{
    public static class HttpTriggerBetter
    {
        [FunctionName("HttpTriggerBetter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogWarning("api/httptrigger called");

            _ = Task.Run(async () =>
            {
                await Helpers.LongRunningFunction(5000, log);
                await Helpers.LongRunningFunction(3000, log);
            });

            string responseMessage = await Helpers.GetResponseMessage(req);
            log.LogWarning($"api/httptrigger done");
            return new OkObjectResult(responseMessage);
        }
    }
}
