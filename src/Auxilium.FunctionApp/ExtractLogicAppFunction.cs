using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Auxilium.Core.LogicApps;
using Auxilium.Core.Utilities;
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
    public static class ExtractLogicAppFunction
    {
        public static Extractor Extractor;

        [FunctionName("ExtractLogicApp")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        //[OpenApiParameter(name: "LogicAppName", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "**LogicAppName** parameter filters for specified logic app name")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            Consoler.Logger = log;
            Consoler.Information("ExtractLogicAppFunction started");
            var authHeader = req.Headers.Single(x => x.Key == "Authorization");
            if (string.IsNullOrEmpty(authHeader.Value)) return new UnauthorizedObjectResult("Invalid Token"); // return HTTP 401 Unauthorized
            var token = authHeader.Value.ToString().Replace("Bearer", "").Trim();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);

            var data = JsonConvert.DeserializeObject<ExtractLogicAppPayload>(requestBody);
            
            try
            {
                Extractor = new Extractor();
                Extractor.Authenticate(token);
                await Extractor.ExtractLogicApp(data.ResourceGroup, data.LogicAppName, data.FailedOnly, data.Export, data.StartDateTime, data.EndDateTime);

            }
            catch (Exception e)
            {
                log.LogError("ERROR ExtractLogicAppFunction",e);
                throw;
            }
           
            return new OkObjectResult(Extractor.Data);
        }
    }
}

