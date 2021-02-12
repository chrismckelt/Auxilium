using System.Collections.Generic;
using System.Threading.Tasks;
using Auxilium.Core.Dtos;

namespace Auxilium.Core.Interfaces
{
	public interface ITenantService
	{
		Task SaveTenantAsync(IList<AzureTenantDto> tenants);
	}
}