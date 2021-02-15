using System;
using System.Threading.Tasks;
using Auxilium.Core;
using Auxilium.Host;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Auxilium.FunctionApp
{
	public static class GetTenants
	{
		[SwaggerIgnore]
		[FunctionName("GetTenants")]
		public static async Task RunAsync(
			[TimerTrigger("0 0 * * * *")] TimerInfo myTimer,
			ILogger log)
		{
			log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
			
			DateTime? startDate = null;

			// _extractor = new Extractor();
			// _extractor.Data = LoadDataFromDisk();
			// //if (!startDate.HasValue) startDate = _extractor.Data.Select(x => x.StartTimeUtc).Max();
			//
			// await _extractor.Run(startDate);
			// await  Export();
			string t = AuthUtil.GetTokenFromConsole();
			Client = new ApiClient(AzureEnvVars.TenantId, AzureEnvVars.SubscriptionId, t);

			var s = await Client.SubscriptionService.ListTenantsAsync();

			log.LogInformation($"There are {s.Value.Count} tenants associated with the client.");
		}

		public static ApiClient Client { get; set; }
	}
}