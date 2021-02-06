using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Auxilium.Core.Utilities;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Auxilium.Core.Tests
{
    public class AzureUtilityTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public AzureUtilityTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Will_parse_resource_group_name_from_full_resource_id()
        {
            const string id = @"/subscriptions/4d123af1-c6dc-488f-8979-57f6607a585b/resourcegroups/blog/providers/microsoft.operationalinsights/workspaces/";
            string res = AzureUtility.ParseResourceGroupName(id);
            _testOutputHelper.WriteLine(res);

            res.ShouldBe("blog");
        }


        [Theory]
        [InlineData("critical", 1)]
        [InlineData("error", 2)]
        [InlineData("warning", 3)]
        public void Will_map_severity_string_to_number(string severityName, int expected)
        {
            var res = AzureUtility.MapSeverity(severityName);
           

            res.ShouldBe(expected);
        }
    }
}
