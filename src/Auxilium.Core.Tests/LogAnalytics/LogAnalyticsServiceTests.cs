using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Auxilium.Core.Tests.LogAnalytics
{
    public class LogAnalyticsServiceTests : TestBase
    {

        private readonly ITestOutputHelper _testOutputHelper;

        public LogAnalyticsServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

        }

        [Fact]
        public async Task List_success()
        {
            var search = new LogAnalyticsQuery();
            var qry = await Client.LogAnalyticsService.LogAnalyticsSearch<dynamic>(search, AzureEnvVars.WorkspaceId);
            _testOutputHelper.WriteLine(qry.Dump());
        }

    }
}
