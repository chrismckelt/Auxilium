using System.Net.Http;
using System.Threading.Tasks;
using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Auxilium.FunctionApp
{
	public static class SwaggerController
	{
		[SwaggerIgnore]
		[FunctionName("Swagger")]
		public static Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "Swagger/json")] HttpRequestMessage req,
			[SwashBuckleClient]ISwashBuckleClient swashBuckleClient)
		{
			return Task.FromResult(swashBuckleClient.CreateSwaggerJsonDocumentResponse(req));
		}

		[SwaggerIgnore]
		[FunctionName("SwaggerUi")]
		public static Task<HttpResponseMessage> Run2(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "Swagger/ui")] HttpRequestMessage req,
			[SwashBuckleClient]ISwashBuckleClient swashBuckleClient)
		{
			return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));
		}
	}
}