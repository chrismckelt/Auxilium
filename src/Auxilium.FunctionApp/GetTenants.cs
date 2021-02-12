using System;
using System.IO;
using System.Threading.Tasks;
using Auxilium.Core.Interfaces;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace Auxilium.FunctionApp
{
	public class GetTenants
	{
		private readonly ITenantService _tenantService;

		public GetTenants(ITenantService tenantService)
		{
			_tenantService = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
		}
		
		[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
		[FunctionName("GetTenantsAsync")]
		[QueryStringParameter("domain", "this is name", DataType = typeof(string), Required = false)]
		public async Task<IActionResult> RunAsync(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
			HttpRequest req, ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");
			string name = req.Query["domain"];

			var result = await _tenantService.GetTenantsAsync();

			return new OkObjectResult(result);
		}
	}
}