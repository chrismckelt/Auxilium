using System;
using System.Linq;
using System.Threading.Tasks;
using Auxilium.Core.Utilities;
using Microsoft.Azure.Management.Sql.Fluent.SqlVirtualNetworkRuleOperations.SqlVirtualNetworkRuleActionsDefinition;

namespace Auxilium.Core.Resources
{
    public class ResourceService : ServiceBase, IResourceService
    {
        public ResourceService(string token = null) : base(token)
        {
        }

        public async Task<IHaveModels<Resource>> ListAsync(string subscriptionId = null)
        {
            var url =
                $"https://management.azure.com/subscriptions/{subscriptionId}/resources?api-version=2019-10-01";
            var results = await Utility.GetResourceAsync<ResourceList>(url, Token)
                .ConfigureAwait(false);
            return results;
        }

        public async Task<IHaveModels<Resource>> ListByResourceGroupAsync(string subscriptionId,
            string resourceGroupName)
        {
            var url =
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/resources?api-version=2019-10-01";
            var results = await Utility.GetResourceAsync<ResourceList>(url, Token)
                .ConfigureAwait(false);
            return results;
        }

        public Task<Resource> GetAsync(string subscriptionId, string resourceGroupName, string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IHaveModels<Resource>> FindByTypeAsync(string subscriptionId, ResourceType resourceType)
        {
            string filter = $"$filter=resourcetype eq '{resourceType.GetDescription()}'";
            var url =
                $"https://management.azure.com/subscriptions/{subscriptionId}/resources?api-version=2019-10-01&{filter}";
            var results = await Utility.GetResourceAsync<ResourceList>(url, Token)
                .ConfigureAwait(false);

            //var value = results.Value.Where(x=>x.Type ==  resourceType.GetDescription());
            //results.Value = value.ToList();
            return results;
        }
    }
}