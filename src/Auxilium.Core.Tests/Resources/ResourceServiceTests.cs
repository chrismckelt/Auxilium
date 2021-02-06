using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Auxilium.Core.Tests.Resources
{
    public class ResourceServiceTests : TestBase
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ResourceServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task List_resources_will_find_items()
        {
            var found = false;
            var subs = await Client.AzureService.Subscriptions.ListAsync().ConfigureAwait(false);
            foreach (var s in subs)
            {
                var rgs = await Client.ResourceGroupService.ListAsync(s.SubscriptionId).ConfigureAwait(false);
                foreach (var rg in rgs.Value)
                {
                    _testOutputHelper.WriteLine("###########################################################");
                    _testOutputHelper.WriteLine($"{rg.Name}");
                    _testOutputHelper.WriteLine("###########################################################");
                    var resources = await Client.ResourceService.ListByResourceGroupAsync(s.SubscriptionId, rg.Name).ConfigureAwait(false);
                    foreach (var r in resources.Value)
                    {
                        try
                        {
                            var diagnostics = await Client.AzureService.DiagnosticSettings.ListByResourceAsync(r.Id);
                            foreach (var diagnosticSetting in diagnostics)
                            {
                                diagnosticSetting.Logs.Each(x => _testOutputHelper.WriteLine($"{r.Type} : {r.Name} : { diagnosticSetting.WorkspaceId} : {x.Category}"));

                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    found = true;
                }
            }
            found.ShouldBeTrue("No resources found");
        }

        [Fact]
        public async Task Filter_by_resource_type_will_find_app_insights_resources()
        {
            var results = await Client.ResourceService.FindByTypeAsync(SubscriptionId, ResourceType.ApplicationInsights);
            foreach (var val in results.Value)
            {
                _testOutputHelper.WriteLine(val.Type);
            }

            results.Value.All(x=>String.Equals(x.Type, ResourceType.ApplicationInsights.GetDescription(), StringComparison.InvariantCultureIgnoreCase)).ShouldBeTrue();
        }
    }
}