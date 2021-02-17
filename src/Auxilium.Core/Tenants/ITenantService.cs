using System.Threading.Tasks;

namespace Auxilium.Core.Tenants
{
    /// <summary>
    /// Extra methods not in .Net Azure SDK
    /// </summary>
    public interface ITenantService
    {
        Task<string> GetTenantIdAsync();
        Task<AzureResultList<AzureTenantValue>> ListTenantsAsync();
    }
}
