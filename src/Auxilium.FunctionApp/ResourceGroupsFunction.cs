using System;
using System.IO;
using System.Threading.Tasks;
using Auxilium.Core;
using Auxilium.Core.LogicApps;
using Auxilium.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Auxilium.FunctionApp
{
    public static class ResourceGroupsFunction
    {
        public static Extractor Extractor;

        [FunctionName("ResourceGroups")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("ResourceGroupsFunction started");

            Extractor = new Extractor();
            await Extractor.Load();

            return new OkObjectResult(Extractor.ResourceGroups);
        }
    }
}
