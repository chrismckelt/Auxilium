using Microsoft.Azure.Management.Fluent;
using Auxilium.Core.LogAnalytics;
using Auxilium.Core.LogicApps;
using Auxilium.Core.ResourceGroups;
using Auxilium.Core.Resources;
using Auxilium.Core.Tenants;
using Auxilium.Core.Workspaces;

namespace Auxilium.Core
{
    public interface IApiClient
    {
        string SubscriptionId { get; }
        IAzure AzureService { get; }
        string Token { get; }
        string TenantId { get; }
        ITenantService TenantService { get; set; }
        IResourceGroupService ResourceGroupService { get; set; }
        IResourceService ResourceService { get; set; }
        ILogicAppService LogicAppService { get; set; }
        IWorkspacesService WorkspacesService { get; set; }
        ILogAnalyticsService LogAnalyticsService { get; set; }
        ILogAnalyticsDataCollector LogAnalyticsDataCollector { get; set; }
        T Create<T>() where T : IHaveAToken;
        void Init();
        void SetAccessToken(string token);
    }
}