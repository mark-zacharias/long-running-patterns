using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Azure.Functions.QueueTrigger
{
    public static class QueueTrigger
    {
        [FunctionName("QueueTrigger")]
        public static async Task Run([QueueTrigger("myqueue-items", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
			log.LogWarning($"C# Queue trigger function started: {myQueueItem}");
			//log.LogInformation("Read Id from message and look up more info in Db");
			
			await Helpers.LongRunningFunction(5000, log);

			var r = new Random();
			if (r.Next(10) > 5)
			{
				throw new Exception("random failure");
			}

			log.LogWarning($"C# Queue trigger function processed: {myQueueItem}");
		}
    }
}
