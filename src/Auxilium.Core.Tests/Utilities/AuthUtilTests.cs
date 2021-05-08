using System.Threading.Tasks;
using Auxilium.Core.Utilities;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Auxilium.Core.Tests.Utilities
{
    public class AuthUtilTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public AuthUtilTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact(Skip = "")]
        public async Task Will_login()
        {
            var url =
                $"https://management.azure.com/subscriptions/{AzureEnvVars.SubscriptionId}/resources?api-version=2019-10-01";
            var appId = "";
            var password = "";
            var tenant = "";

            // var token = await Auxilium.Core.Utilities.Authenticator.GetAuthTokenForAadNativeApp(appId, password, tenant, url, id);
            //token.ShouldNotBeNull();
            var t = await AuthUtil.GetBearer(tenant, appId, password);
            t.ShouldNotBeNull();
        }


      
    }
}
