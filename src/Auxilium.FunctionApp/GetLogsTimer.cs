using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Auxilium.FunctionApp
{
	public static class GetLogsTimer
	{
		[SwaggerIgnore]
		[FunctionName("GetLogsTimer")]
		public static async Task RunAsync(
			[TimerTrigger("0 * */1 * * *")] TimerInfo myTimer,
			ILogger log)
		{
			log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
			
		}
	}
}