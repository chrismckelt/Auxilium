using System.Threading.Tasks;

namespace Auxilium.Core.Resources
{
    /// <summary>
    /// Extra methods not in .Net Azure SDK
    /// </summary>
    public interface IResourceService : IBaseService<Resource>
    {
        Task<IHaveModels<Resource>> FindByTypeAsync(string subscriptionId, ResourceType resourceType);
    }
}
