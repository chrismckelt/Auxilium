using System;
using System.IO;
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
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("TenantsFunction started");

            string name = req.Query["name"];

           // DateTime? startDate = null;

            // _extractor = new Extractor();
            // _extractor.Data = LoadDataFromDisk();
            // //if (!startDate.HasValue) startDate = _extractor.Data.Select(x => x.StartTimeUtc).Max();
            //
            // await _extractor.Run(startDate);
            // await  Export();
            string token = AuthUtil.GetTokenFromConsole();
            Client = new ApiClient(AzureEnvVars.TenantId, AzureEnvVars.SubscriptionId, token);

            var resultList = await Client.TenantService.ListTenantsAsync();

            log.LogInformation($"There are {resultList.Value.Count} tenants associated with the client.");

            return new OkObjectResult(resultList);
        }
    }
}
