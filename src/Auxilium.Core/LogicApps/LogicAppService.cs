using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Auxilium.Core.Utilities;
using Newtonsoft.Json;

namespace Auxilium.Core.LogicApps
{
    /// <summary>
    /// The MS Nuget package is not .Net CORE - hence go direct to REST API
    /// </summary>
    public class LogicAppService : ServiceBase, ILogicAppService
    {
        public IList<LogicAppWorkflowRun> WorkflowRuns { get; private set; }
        public int PageCountToRetrieve { get; set; }
        public LogicAppRunFilter Filter { get; set; }
        public string LogicAppName { get; set; }

        public LogicAppService(string token = null) : base(token)
        {
            PageCountToRetrieve = 50;
            Filter = LogicAppRunFilter.Failed;

        }

        public async Task<AzureLogicApps> ListAsync(string subscriptionId, string resourceGroupName)
        {
            var url =
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Logic/workflows?api-version=2016-06-01";
            
            var items = new List<AzureLogicAppValue>();
            var breaker = 0;
            var result = await Utility.GetResourceAsync<AzureLogicApps>(url, Token).ConfigureAwait(false);
            result.Value.ToList().ForEach(items.Add);

            var link = result.NextLink;
            while (!string.IsNullOrWhiteSpace(link) && breaker < PageCountToRetrieve)
            {
                Console.WriteLine("Logic app runs for page " + breaker);
                Console.WriteLine(link);
                result = await Utility.GetResourceAsync<AzureLogicApps>(link, Token).ConfigureAwait(false);
                link = result.NextLink;

                foreach (var x in result.Value) items.Add(x);
                breaker++;
            }

            return new AzureLogicApps
            {
                Value = items
            };
        }

        public async Task<AzureLogicAppWorkflowRuns> WorkflowRunListAsync(string subscriptionId,
            string resourceGroupName, string logicAppName, DateTime? startTimeBegin = new DateTime?(), DateTime? startTimeEnd = new DateTime?())
        {
            LogicAppName = logicAppName;

            //Console.Clear();
            var runs = new List<LogicAppWorkflowRunValue>();

            var search = $"$filter=status eq '{Filter.GetDescription()}'";
            if (startTimeBegin != null)
            {
                search += $" and startTime ge { startTimeBegin.Value.ToUniversalTime() :yyyy-MM-ddTHH:mm:ssZ}";
            }
            if (startTimeEnd != null)
            {
                search += $" and startTime le { startTimeEnd.Value.ToUniversalTime() :yyyy-MM-ddTHH:mm:ssZ}";
            }

            search += "&orderby=startTime desc";

            var url =
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Logic/workflows/{logicAppName}/runs?api-version=2016-06-01&{search}";
            var breaker = 0;
            var result = await Utility.GetResourceAsync<AzureLogicAppWorkflowRuns>(url, Token);
            result.Value.ToList().ForEach(runs.Add);

            var link = result.NextLink;
            while (!string.IsNullOrWhiteSpace(link) && breaker < PageCountToRetrieve)
            {
                Console.WriteLine("Logic app runs for page " + breaker);
                Console.WriteLine(link);
                result = await Utility.GetResourceAsync<AzureLogicAppWorkflowRuns>(link, Token);
                link = result.NextLink;

                foreach (var x in result.Value) runs.Add(x);
                breaker++;
            }

            return new AzureLogicAppWorkflowRuns
            {
                Value = runs
            };
        }


        public async Task<LogicAppRunAction> WorkflowRunGetActionAsync(string subscriptionId,
            string resourceGroupName, string logicAppName, string runId, string actionName)
        {
            
            LogicAppName = logicAppName;
             
            //Console.Clear();
            var runActionResult = new LogicAppRunAction()
            {
                SubscriptionId = subscriptionId,
                ResourceGroupName = resourceGroupName,
                LogicAppName = logicAppName,
                RunId = runId,
                ActionName = actionName,
            };

            var url =
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Logic/workflows/{logicAppName}/runs/{runId}/actions/{actionName}?api-version=2016-06-01";

            var actionValue = await Utility.GetResourceAsync<ActionValue>(url, Token);

            if (actionValue != null)
            {
                //var contentUrl = actionValue.Properties.OutputsLink.Uri;
                //var actionValue = await Utility.GetResource<ActionValue>(url, Token);

                var inputContent = await Utility.TryExtractContentAsync(actionValue.Properties?.InputsLink?.Uri);
                var outputContent = await Utility.TryExtractContentAsync(actionValue.Properties?.OutputsLink?.Uri);

                runActionResult.StartTimeUtc = actionValue.Properties?.StartTime;
                runActionResult.EndTimeUtc = actionValue.Properties?.EndTime;
                runActionResult.Id = actionValue.Id;
                runActionResult.InputLink = actionValue.Properties?.InputsLink?.Uri;
                runActionResult.InputContentData = inputContent;
                runActionResult.OutputLink = actionValue.Properties?.OutputsLink?.Uri;
                runActionResult.OutputContentData = outputContent;
                runActionResult.Status = actionValue.Properties?.Status;
                runActionResult.StatusCode = actionValue.Properties?.Code;
            }

            return runActionResult;
        }


        public async Task<AzureLogicAppWorkflowRuns> WorkflowRunGetAsync(string subscriptionId,
            string resourceGroupName, string workflowName, string runName)
        {
            var url =
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Logic/workflows/{workflowName}/runs/{runName}/actions?api-version=2016-06-01";
            return await Utility.GetResourceAsync<AzureLogicAppWorkflowRuns>(url, Token);
        }

        public async Task<AzureLogicAppWorkflowTriggers> WorkflowTriggersListAsync(
            string subscriptionId, string resourceGroupName, string workflowName)
        {
            //string filter = "properties.status eq 'Success'";
            var search = "";// "&`$filter=properties.status eq 'Success'";
            var url =
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Logic/workflows/{workflowName}/triggers?api-version=2016-06-01&{search}";
            return await Utility.GetResourceAsync<AzureLogicAppWorkflowTriggers>(url, Token);
        }


        /// <summary>
        /// eg manual or When_a_message_is_received_in_topic_subscription
        /// ///     https://docs.microsoft.com/en-us/rest/api/logic/workflowtriggerhistories/list
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="trigger"></param>
        /// <param name="resourceGroupName"></param>
        /// <param name="logicAppName"></param>
        /// <returns></returns>
        public async Task<AzureLogicAppWorkflowTriggerHistoryRun> WorkflowTriggerHistoryListAsync(string subscriptionId, string trigger,
            string resourceGroupName, string logicAppName)
        {
            LogicAppName = logicAppName;
            
            var url =
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Logic/workflows/{logicAppName}/triggers/{trigger}/histories?api-version=2016-06-01";
            Console.WriteLine("WorkflowTriggerHistoryList");
            Console.WriteLine(url);
            return await Utility.GetResourceAsync<AzureLogicAppWorkflowTriggerHistoryRun>(url, Token);
        }


        //     resubmit a logic app run
        //     https://docs.microsoft.com/en-us/rest/api/logic/workflowtriggerhistories/resubmit
        public async Task<LogicAppResubmittedRun> ResubmitAsync(string subscriptionId,
            string resourceGroupName, string logicAppName, string triggerHistoryRunId, string triggerName = "manual")
        {
            string url = $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Logic/workflows/{logicAppName}/triggers/{triggerName}/histories/{triggerHistoryRunId}/resubmit?api-version=2016-06-01";
            Console.WriteLine($"Resubmit ${url}");

            using (var client = new HttpClient())
            {
                var logicAppResubmittedRun = new LogicAppResubmittedRun()
                {
                    SubscriptionId = subscriptionId,
                    ResourceGroupName = resourceGroupName,
                    LogicAppName = logicAppName,
                    Trigger = triggerName,
                    OldRunId = triggerHistoryRunId,
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var postData = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, postData);

                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    //try to get the replay run id
                    if (response.Headers.Contains("x-ms-workflow-run-id"))
                    {
                        var newRunId = response.Headers.GetValues("x-ms-workflow-run-id").FirstOrDefault();
                        var correlationId = response.Headers.GetValues("x-ms-correlation-id").FirstOrDefault();
                        if (!string.IsNullOrEmpty(newRunId))
                        {
                            logicAppResubmittedRun.NewRunId = newRunId;
                        }

                        logicAppResubmittedRun.Correlation = correlationId;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    logicAppResubmittedRun.Content = content.Dump();
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }

                logicAppResubmittedRun.Status = response.StatusCode;
                logicAppResubmittedRun.IsSuccessStatusCode = response.IsSuccessStatusCode;

                return logicAppResubmittedRun;
            }
        }

        private async Task<string> GetContent(string uri)
        {
            if (string.IsNullOrEmpty(uri)) return null;
            using (var client = new HttpClient())
            {
                //       client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);  --> this causes an ERROR ???
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("GetContent");
                Console.WriteLine(uri);
                Console.WriteLine("-----------------------------------------");
                throw new ApplicationException(response.ReasonPhrase);
            }
        }

        public async Task<IEnumerable<LogicAppWorkflowRun>> FetchContentAsync(string subscriptionId, string resourceGroupName, string logicAppName, AzureLogicAppWorkflowRuns runs)
        {
            var data = new List<LogicAppWorkflowRun>();
            foreach (var value in runs.Value)
            {
                // new item
                var inputContent =
                    await Utility.TryExtractContentAsync(value.Properties.Trigger?.InputsLink
                        ?.Uri);
                var outputContent =
                    await Utility.TryExtractContentAsync(
                        value.Properties.Trigger?.OutputsLink?.Uri);
                var item = new LogicAppWorkflowRun
                {
                    CreationTime = DateTime.UtcNow,
                    CreatorUserId = -1,

                    LogicAppName = logicAppName,

                    ResourceGroupName = resourceGroupName,
                    SubscriptionId = subscriptionId,

                    RunId = value.Id,
                    Name = value.Name,
                    StartTimeUtc = value.Properties.StartTime,
                    EndTimeUtc = value.Properties.EndTime,
                    Status = value.Properties.Status,
                    Trigger = JsonConvert.SerializeObject(value.Properties.Trigger),
                    Outputs = JsonConvert.SerializeObject(value.Properties.Outputs),
                    Correlation = JsonConvert.SerializeObject(value.Properties.Correlation),
                    TriggerInput = value.Properties.Trigger?.InputsLink?.Uri,
                    TriggerOutput = value.Properties.Trigger?.OutputsLink?.Uri,
                    OutputContentData =
                        Utility.TryParseHttpResponseContentData(outputContent),
                    InputContentData = Utility.TryParseHttpResponseContentData(inputContent),
                    ErrorCode = value.Properties.Error?.Code,
                    ErrorMessage = value.Properties.Error?.Message,
                    Code = value.Properties.Code,
                    ExtensionData = JsonConvert.SerializeObject(value)
                };

                //WorkflowRuns.InsertOnSubmit(item);
                //SubmitChanges();
                data.Add(item);
            }

            return data;
        }
    }
}