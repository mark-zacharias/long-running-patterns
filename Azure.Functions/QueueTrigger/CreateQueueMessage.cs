using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Queues;
using System.Text;

namespace Azure.Functions.QueueTrigger
{
    public static class CreateQueueMessage
    {
        [FunctionName("CreateQueueMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string message = req.Query["message"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            message = message ?? data?.message;

            var storageConnection = Environment.GetEnvironmentVariable("AzureWebJobsStorage", EnvironmentVariableTarget.Process);
            var queueClient = new QueueClient(storageConnection, "myqueue-items");
            if (!await queueClient.ExistsAsync()) //check if exists explicitly to avoid 409 trace information in the logs.
            {
                await queueClient.CreateIfNotExistsAsync();
            }

            var base64String = GetUTF8String(message);

            var receipt = await queueClient.SendMessageAsync(base64String);
            return new OkObjectResult ($"Added a new message with id {receipt.Value.MessageId}");
        }

        private static string GetUTF8String(string message)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        }
    }
}
