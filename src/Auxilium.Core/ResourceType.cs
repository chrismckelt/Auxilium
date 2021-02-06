using System.ComponentModel;

namespace Auxilium.Core
{
    public enum ResourceType
    {
        [Description("Microsoft.Logic/workflows")]
        Workflows,

        [Description("Microsoft.ApiManagement/service")]
        Service,

        [Description("Microsoft.KeyVault/vaults")]
        Vaults,

        [Description("Microsoft.ServiceBus/namespaces")]
        Namespaces,
        [Description("Microsoft.Web/sites")] Sites,

        [Description("Microsoft.Web/serverfarms")]
        Serverfarms,

        [Description("Microsoft.Sql/servers/databases")]
        Databases,

        [Description("Microsoft.Network/applicationGateways")]
        ApplicationGateways,

        [Description("Microsoft.Network/networkSecurityGroups")]
        NetworkSecurityGroups,

        [Description("Microsoft.ContainerService/managedClusters")]
        ManagedClusters,

        [Description("Microsoft.EventGrid/topics")]
        Topics,

        [Description("Microsoft.ContainerService/managedClusters/agentPools")]
        AgentPools,

        [Description("Microsoft.EventGrid/systemTopics")]
        SystemTopics,

        [Description("Microsoft.Cache/Redis")] Redis,

        [Description("Microsoft.Insights/components")]
        ApplicationInsights

        //_testOutputHelper.WriteLine($"[Description(\"{x}\")]");
        //var split = x.Split('/');
        //_testOutputHelper.WriteLine($"{split[1].ToTitleCase()},");
    }
}