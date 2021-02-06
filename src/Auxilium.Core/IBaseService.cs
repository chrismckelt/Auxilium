using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auxilium.Core
{
    public interface IBaseService<T> where T : new()
    {
        Task<IHaveModels<T>> ListAsync(string subscriptionId = null);
        Task<IHaveModels<T>> ListByResourceGroupAsync(string subscriptionId, string resourceGroupName);
        Task<T> GetAsync(string subscriptionId, string resourceGroupName, string name);
    }
}
