using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Auxilium.Core.Utilities;

namespace Auxilium.Core.ResourceGroups
{
    public class ResourceGroupService : ServiceBase, IResourceGroupService
    {
        private const string ResourceGroupPattern = @"resourceGroups\/([\w-]+)\/";
        public ResourceGroupService(string token = null) : base(token)
        {
        }

        public async Task<AzureResultList<AzureResourceGroupValue>> ListAsync(string subscriptionId)
        {
            var url =
                $"https://management.azure.com/subscriptions/{subscriptionId}/resourcegroups?api-version=2018-05-01";
            var results = await Utility.GetResourceAsync<AzureResultList<AzureResourceGroupValue>>(url, Token).ConfigureAwait(false);
            return results;
        }

        public async Task<AzureResultList<AzureResourceValue>> ListResourceGroupsAsync(string subscriptionId, params string[] resourceGroups)
        {
            if (resourceGroups.Length == 0)
            {
                var url =
                    $"https://management.azure.com/subscriptions/{subscriptionId}/resources?api-version=2019-08-01";
                var results = await Utility.GetResourceAsync<AzureResultList<AzureResourceValue>>(url, Token).ConfigureAwait(false);

                Parallel.ForEach(results.Value, r =>
                {
                    var match = Regex.Match(r.Id, ResourceGroupPattern);
                    if (match.Success)
                    {
                        r.ResourceGroupName = match.Groups[1].Value;
                    }
                });

                return results;
            }
            else
            {
                var results = new AzureResultList<AzureResourceValue> { Value = new List<AzureResourceValue>() };
                foreach (var resourceGroupName in resourceGroups)
                {
                    var url =
                        $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/resources?api-version=2019-08-01";
                    var items = await Utility.GetResourceAsync<AzureResultList<AzureResourceValue>>(url, Token).ConfigureAwait(false);
                    Parallel.ForEach(items.Value, r =>
                    {
                        r.ResourceGroupName = resourceGroupName;
                        results.Value.Add(r);
                    });
                }

                return results;
            }
        }
    }
}