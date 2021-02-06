using System.Threading.Tasks;

namespace Auxilium.Core.Subscriptions
{
    /// <summary>
    /// Extra methods not in .Net Azure SDK
    /// </summary>
    public interface ISubscriptionService
    {
        Task<string> GetTenantIdAsync();
        Task<AzureResultList<AzureTenantValue>> ListTenantsAsync();
    }
}
