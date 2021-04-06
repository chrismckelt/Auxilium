using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Auxilium.Core;
using Auxilium.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Auxilium.FunctionApp
{
    public static class TenantsFunction
    {
        public static ApiClient Client { get; set; }

        [FunctionName("Tenants")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("TenantsFunction started");

            var authHeader = req.Headers.Single(x => x.Key == "Authorization");
            if (string.IsNullOrEmpty(authHeader.Value)) return new UnauthorizedObjectResult("Invalid Token"); // return HTTP 401 Unauthorized
            var token = authHeader.Value.ToString().Replace("Bearer", "").Trim();

            Client = new ApiClient(AzureEnvVars.TenantId, AzureEnvVars.SubscriptionId, token);

            var resultList = await Client.TenantService.ListTenantsAsync();

            log.LogInformation($"There are {resultList.Value.Count} tenants associated with the client.");

            return new OkObjectResult(resultList);
        }
    }
}
