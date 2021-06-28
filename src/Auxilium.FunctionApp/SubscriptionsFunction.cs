using System;
using System.IO;
using System.Linq;
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
    public static class SubscriptionsFunction
    {
        public static Extractor Extractor;

        [FunctionName("Subscriptions")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("SubscriptionsFunction started");

            var authHeader = req.Headers.SingleOrDefault(x => x.Key == "Authorization");
            if (authHeader.Key == null || string.IsNullOrEmpty(authHeader.Value)) return new UnauthorizedObjectResult("Invalid Token"); // return HTTP 401 Unauthorized
            var token = authHeader.Value.ToString().Replace("Bearer", "").Trim();

            Extractor = new Extractor();
            Extractor.Authenticate(token);
            await Extractor.Load();

            return new OkObjectResult(Extractor.Subscriptions);
        }
    }
}
