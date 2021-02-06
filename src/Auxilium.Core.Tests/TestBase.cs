using System;
using Auxilium.Host;
using AuthUtil = Auxilium.Core.Utilities.AuthUtil;

namespace Auxilium.Core.Tests
{
    public abstract class TestBase
    {
        protected static readonly string TenantId = AzureEnvVars.TenantId;  //https://www.whatismytenantid.com/
        protected static readonly string SubscriptionId = AzureEnvVars.SubscriptionId;

        protected static ApiClient Client { get; private set; }

        protected TestBase()
        {
            var client = new ApiClient(TenantId, SubscriptionId);
            var token = AuthUtil.GetTokenFromConsole();
            client.SetAccessToken(token);
            Client = client;
        }

    }
}
