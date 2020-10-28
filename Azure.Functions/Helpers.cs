using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Functions
{
    class Helpers
	{public static async Task<string> GetResponseMessage(HttpRequest req)
		{
			string name = req.Query["name"];

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);

			name = name ?? data?.name;


			string responseMessage = string.IsNullOrEmpty(name)
				? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
				: $"Hello, {name}. This HTTP triggered function executed successfully.";
			return responseMessage;
		}

		public static async Task LongRunningFunction(int delay, ILogger log)
		{
			log.LogInformation($"starting {delay / 1000} second long running process");
			await Task.Delay(delay);
			log.LogInformation($"finishing {delay / 1000} long running process");
			return;
		}
	}
}
