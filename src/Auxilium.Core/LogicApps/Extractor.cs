using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Auxilium.Core.Utilities;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Newtonsoft.Json;

namespace Auxilium.Core.LogicApps
{
    public class Extractor
    {
        public static IApiClient Client { get; set; }
        public static string Token { get; set; }

        public List<LogicAppExtract> Data { get; set; } = new List<LogicAppExtract>();
        public IPagedCollection<ISubscription> Subscriptions { get; set; }
		public IPagedCollection<IResourceGroup> ResourceGroups { get; set; }
		public IList<KeyValuePair<string, string>> LogicApps = new List<KeyValuePair<string, string>>();

        public async Task Run(DateTime? startDateTime = null)
        {
            //Token = ApiClient.GetToken();
            Token = AuthUtil.GetTokenFromConsole();
            Client = new ApiClient(AzureEnvVars.TenantId, AzureEnvVars.SubscriptionId, Token);

            await Load();
           
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
          
			await ExtractAll(true, startDateTime.GetValueOrDefault(DateTime.UtcNow.Date.AddDays(-1)).AddHours(-1), DateTime.UtcNow);
            stopwatch.Stop();
            Console.WriteLine($"Total Minutes: {stopwatch.Elapsed.TotalMinutes}");
		}

		async Task Load()
		{
            Subscriptions = await Client.AzureService.Subscriptions.ListAsync();
            Subscriptions.Dump("Subscriptions");
            ResourceGroups = await Client.AzureService.ResourceGroups.ListAsync(true);
            ResourceGroups.Dump("Resource Groups");
			await GetLogicApps();
		}
        public async System.Threading.Tasks.Task GetLogicApps()
        {
            foreach (var rg in ResourceGroups.Select(x => x.Name).Distinct())
            {
                var r = await Client.LogicAppService.ListAsync(AzureEnvVars.SubscriptionId, rg);

                foreach (var x in r.Value)
                {
                    LogicApps.Add(new KeyValuePair<string, string>(rg, x.Name));
                }
            }
        }

        async Task ReplayFailed(IList<Poco> failed)
		{
			foreach (var f in failed)
			{
				try
				{
					await ExtractLogicApps(f.ResourceGroup, f.LogicAppName, false, true);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					Token = ApiClient.GetToken();
					Client = new ApiClient(AzureEnvVars.TenantId, AzureEnvVars.SubscriptionId, Token);
					await ExtractLogicApps(f.ResourceGroup, f.LogicAppName, false, true);
				}
			}
		}

		async Task Replay(IEnumerable<string> failedRunIds)
		{
			foreach (var f in failedRunIds)
			{
				var item = Data.First(x => x.RunId == f);
				var rg = LogicApps.Single(x => x.Value == item.LogicAppName);
				Console.WriteLine($"{AzureEnvVars.SubscriptionId} {rg.Key}  {item.LogicAppName} {item.ActionName} {item.RunId}");
				//string triggerName = "manual";
				var run = await Client.LogicAppService.ResubmitAsync(AzureEnvVars.SubscriptionId, rg.Key, item.LogicAppName, f);
				Console.WriteLine(run.IsSuccessStatusCode);
			}
		}

		public async System.Threading.Tasks.Task ExtractAll(bool failedOnly = false, DateTime? startDateTime=null, DateTime? endDateTime=null, bool export = true)
		{
            foreach (var x in LogicApps)
			{
				try
				{
					await ExtractLogicApps(x.Key, x.Value, failedOnly, export, startDateTime:startDateTime,endDateTime:endDateTime);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
                    Token = ApiClient.GetToken();
                    Client = new ApiClient(AzureEnvVars.TenantId, AzureEnvVars.SubscriptionId, Token);
				}
			}
		}

		public async System.Threading.Tasks.Task ExtractLogicApps(string rgName, string logicAppName, bool failedOnly=false, bool export = true, DateTime? startDateTime=null, DateTime? endDateTime=null)
		{
            Consoler.Information($"*** Extract {logicAppName} ***");
			var runs = await Client.LogicAppService.WorkflowRunListAsync(AzureEnvVars.SubscriptionId, rgName, logicAppName,startTimeBegin:startDateTime, startTimeEnd:endDateTime);

			foreach (var r in runs.Value.Take(500).Select(x => x.Name).Distinct())
			{
				//string runid = "08586013893585485487155006807CU03";
				var target = await Client.LogicAppService.WorkflowRunGetAsync(AzureEnvVars.SubscriptionId, rgName, logicAppName, r);
				IList<LogicAppWorkflowRunValue> workflowRunValues = target.Value;

                if (failedOnly)
                {
                    workflowRunValues = target.Value.Where(x => x.Properties.Status == "Failed").ToList();
                    Consoler.Information($"filtering {target.Value.Count} for failed {workflowRunValues.Count}");
                }

                foreach (var value in workflowRunValues.Select(x => x.Name).Distinct())
				{
					var act = await Client.LogicAppService.WorkflowRunGetActionAsync(AzureEnvVars.SubscriptionId, rgName,
						logicAppName, r, value);

                    var extract = new LogicAppExtract
                    {
                        LogicAppName = logicAppName,
                        ResourceGroup = rgName,
                        Correlation = act.Correlation,
                        RunId = act.RunId,
                        ActionName = act.ActionName,
                        Input = act.InputContentData,
                        InputLink = act.InputLink,
                        Output = act.OutputContentData,
                        OutputLink = act.OutputLink,
                        Status = act.Status,
						StatusCode = act.StatusCode,
                        StartTimeUtc = act.StartTimeUtc,
                        EndTimeUtc = act.EndTimeUtc

                    };

                    if (!Data.Any(x => x.RunId == extract.RunId && x.ActionName == extract.ActionName))
                    {
                        Data.Add(extract);
                        if (export)
                        {
                            // send to log analytics
                            await Client.LogAnalyticsDataCollector.Collect(extract);
                        }
                    }
                }
			}
        }

        async Task Search()
        {
            var query = new LogAnalyticsQuery()
            {
                Query = Kql.Search
            };

            await Client.LogAnalyticsService.LogAnalyticsSearch<dynamic>(query, AzureEnvVars.WorkspaceId);
        }

		public class Poco
		{
			public string ResourceGroup { get; set; }
			public string LogicAppName { get; set; }
			public string RunId { get; set; }
		}

        public class Api
        {

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class PathTemplate
        {

            [JsonProperty("template")]
            public string Template { get; set; }

            [JsonProperty("parameters")]
            public Parameters Parameters { get; set; }
        }

        public class QueryParameter
        {

            [JsonProperty("Key")]
            public string Key { get; set; }

            [JsonProperty("Value")]
            public object Value { get; set; }
        }

        public class Body
        {

            [JsonProperty("DatabaseName")]
            public string DatabaseName { get; set; }

            [JsonProperty("QueryParameters")]
            public IList<QueryParameter> QueryParameters { get; set; }

            [JsonProperty("ServerName")]
            public string ServerName { get; set; }

            [JsonProperty("TemplateFileName")]
            public string TemplateFileName { get; set; }
        }

        public class Payload
        {
            [JsonProperty("api")]
            public Api Api { get; set; }

            [JsonProperty("method")]
            public string Method { get; set; }

            [JsonProperty("pathTemplate")]
            public PathTemplate PathTemplate { get; set; }

            [JsonProperty("subscriptionKey")]
            public string SubscriptionKey { get; set; }

            [JsonProperty("body")]
            public Body Body { get; set; }
        }
    }
}
