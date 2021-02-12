using System;
using System.Linq;
using System.Threading.Tasks;
using Auxilium.Core;
using Auxilium.Core.Dtos;
using Auxilium.Core.Interfaces;
using Auxilium.Core.Utilities;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Auxilium.FunctionApp
{
	public class GetLogsTimer
	{
		private readonly ITenantService _tenantService;

		public GetLogsTimer(ITenantService tenantService)
		{
			_tenantService = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
		}
		
		[SwaggerIgnore]
		[FunctionName("GetLogsTimer")]
		public async Task RunAsync(
			[TimerTrigger("0 0 */1 * * *")] TimerInfo myTimer,
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

			var dtos = s.Value.Select(x => new AzureTenantDto
			{
				Domains = x.Domains,
				TenantId = x.Id,
				Id = x.TenantId,
				Name = x.DisplayName,
				PartitionKey = "/tennantid",
				DisplayName = x.DisplayName
			}).ToList();

			await _tenantService.SaveTenantAsync(dtos);
		}

		public static ApiClient Client { get; set; }
	}
}