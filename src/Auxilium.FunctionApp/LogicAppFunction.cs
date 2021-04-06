using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Auxilium.Core.LogicApps;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Auxilium.FunctionApp
{
    public static class LogicAppFunction
    {
        public static Extractor Extractor;

        [FunctionName("LogicApps")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("LogicAppFunction started");
            var authHeader = req.Headers.Single(x => x.Key == "Authorization");
            if (string.IsNullOrEmpty(authHeader.Value)) return new UnauthorizedObjectResult("Invalid Token"); // return HTTP 401 Unauthorized
            var token = authHeader.Value.ToString().Replace("Bearer", "").Trim();
            Extractor = new Extractor();
            Extractor.Authenticate(token);
            await Extractor.Load();
           
            return new OkObjectResult(Extractor.LogicApps);
        }
    }
}

