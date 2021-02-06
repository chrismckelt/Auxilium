using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using Auxilium.Core.LogicApps;
using Xunit;
using Xunit.Abstractions;

namespace Auxilium.Core.Tests.LogicApps
{
    public class LogicAppServiceTests : TestBase
    {
        private readonly ITestOutputHelper _testOutputHelper;

        // logic app debug
        private const string SubscriptionName = "Azure_GX_Production"; // todo environment variable this
        private const string ResourceGroupName = "gx-p-arg-pos-dogwash-rms";
        private const string LogicAppName = "GX-Syd-P-ALA-DogWash-ProcessTransactions";

        public LogicAppServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

        }

        [Fact]
        public async Task List_success()
        {
            var subs = await Client.AzureService.Subscriptions.ListAsync();
            subs.Dump("Subscriptions");
            var sub = subs.Single(x => x.DisplayName == SubscriptionName);

            var rgs = await Client.AzureService.ResourceGroups.ListAsync(true);
            rgs.Dump("Resource Groups");


            var runs = await Client.LogicAppService.WorkflowRunListAsync(SubscriptionId, ResourceGroupName, LogicAppName);

            runs.Value.Dump("Runs");

            var failed = runs.Value.Take(3);

            failed.Dump("Failed");

            var extracted = await Client.LogicAppService.FetchContentAsync(SubscriptionId, ResourceGroupName,
                LogicAppName,
                new AzureLogicAppWorkflowRuns() { Value = failed.ToList() });

            extracted.Dump();
        }

        [Fact]
        public async Task Get_success()
        {
            var runs = await Client.LogicAppService.WorkflowRunListAsync(SubscriptionId, ResourceGroupName, LogicAppName);

            var list = new List<LogicAppExtract>();

            foreach (var r in runs.Value)
            {
                //string runid = "08586013893585485487155006807CU03";
                var target = await Client.LogicAppService.WorkflowRunGetAsync(SubscriptionId, ResourceGroupName, LogicAppName, r.Name);

                var failed = target.Value.Where(x => x.Properties.Status == "Failed");

                foreach (var value in failed)
                {
                    var act = await Client.LogicAppService.WorkflowRunGetActionAsync(SubscriptionId, ResourceGroupName,
                        LogicAppName, r.Name, value.Name);

                    if (!string.IsNullOrEmpty(act.OutputContentData))
                    {
                        var extract = new LogicAppExtract
                        {
                            LogicAppName = LogicAppName,
                            ActionName = act.ActionName,
                            Input = act.InputContentData,
                            Output = act.OutputContentData
                        };

                        list.Add(extract);

                    }
                }
            }

            list.ShouldNotBeEmpty();
            list.SaveToFile(@"c:\temp\export.json");

        }

    }
}