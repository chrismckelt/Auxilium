using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auxilium.Core.Utilities;

namespace Auxilium.Core.Workspaces
{
    public class WorkspacesService : ServiceBase, IWorkspacesService
    {
        public WorkspacesService(string token = null) : base(token)
        {
            
        }

        public async Task<IHaveModels<WorkspaceModel>> ListAsync(string subscriptionId = null)
        {
            string url = $"https://management.azure.com/subscriptions/{subscriptionId}/providers/Microsoft.OperationalInsights/workspaces?api-version=2020-08-01";
            var results = await Utility.GetResourceAsync<WorkspaceList>(url, Token)
                .ConfigureAwait(false);
            return results;
        }

        public async Task<IHaveModels<WorkspaceModel>> ListByResourceGroupAsync(string subscriptionId, string resourceGroupName)
        {
            string url = $"https://management.azure.com/subscriptions/{subscriptionId}/resourcegroups/{resourceGroupName}/providers/Microsoft.OperationalInsights/workspaces?api-version=2020-08-01";
            var results = await Utility.GetResourceAsync<WorkspaceList>(url, Token)
                .ConfigureAwait(false);
            return results;
        }

        public async Task<WorkspaceModel> GetAsync(string subscriptionId, string resourceGroupName, string name)
        {
            string url = $"https://management.azure.com/subscriptions/{subscriptionId}/resourcegroups/{resourceGroupName}/providers/Microsoft.OperationalInsights/workspaces/{name}?api-version=2020-08-01";
            var results = await Utility.GetResourceAsync<WorkspaceModel>(url, Token)
                .ConfigureAwait(false);
            return results;
        }
    }
}