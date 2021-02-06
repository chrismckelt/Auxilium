using System.Threading.Tasks;

namespace Auxilium.Core.ResourceGroups
{
    /// <summary>
    /// Extra methods not in .Net Azure SDK
    /// </summary>
    public interface IResourceGroupService
    {
        Task<AzureResultList<AzureResourceGroupValue>> ListAsync(string subscriptionId);

        Task<AzureResultList<AzureResourceValue>> ListResourceGroupsAsync(string subscriptionId,
            params string[] resourceGroups);
    }
}
