using System.Linq;
using System.Threading.Tasks;
using Auxilium.Core.Utilities;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Auxilium.Core.Tests.Workspaces
{
    public class WorkspacesServiceTest : TestBase
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WorkspacesServiceTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task List_success()
        {
            var ws = await Client.WorkspacesService.ListAsync(SubscriptionId);
            foreach (var w in ws.Value) _testOutputHelper.WriteLine(JsonConvert.SerializeObject(w));
        }

        [Fact]
        public async Task Get_success()
        {
            var ws = await Client.WorkspacesService.ListAsync(SubscriptionId);
            var resourceGroupName = AzureUtility.ParseResourceGroupName(ws.Value.First().Id);
            var found = await Client.WorkspacesService.GetAsync(SubscriptionId,
                resourceGroupName, ws.Value.First().Name);

            found.ShouldNotBeNull();
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(found));
        }
    }
}