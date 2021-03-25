using System.IO;
using System.Threading.Tasks;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Auxilium.FunctionApp
{
	public static class TestHttpSwagger
	{
		[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
		[FunctionName("TestHttpSwagger")]
		[QueryStringParameter("name", "this is name", DataType = typeof(string), Required = false)]
		public static async Task<IActionResult> RunAsync(
			[HttpTrigger(AuthorizationLevel.Function, "get", "put", Route = null)]
			HttpRequest req, ILogger log)
		{
			log.LogInformation("TestHttpSwagger started");

			string name = req.Query["name"];

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			name ??= data?.name;

			return name != null
				? (ActionResult) new OkObjectResult($"SWAGGER OK, {name}")
				: new BadRequestObjectResult("BAD data - no name in query string");
			
		}
	}
}