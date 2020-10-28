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
    public static class HttpTriggerBetterYet
    {
        [FunctionName("HttpTriggerBetterYet")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
		{
			log.LogWarning("api/httptriggerbetteryet called");

			_ = Task.Run(async () =>
			{
				var taskOne = Helpers.LongRunningFunction(5000, log);
				var taskTwo = Helpers.LongRunningFunction(3000, log);

				await Task.WhenAll(new Task[] { taskOne, taskTwo });
				log.LogInformation("all tasks done");
			});

			string responseMessage = await Helpers.GetResponseMessage(req);
			log.LogWarning($"api/httptrigger done");
			return new OkObjectResult(responseMessage);
		}
    }
}
