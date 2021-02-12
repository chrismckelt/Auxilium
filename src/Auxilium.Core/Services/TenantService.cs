using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auxilium.Core.Dtos;
using Auxilium.Core.Interfaces;

namespace Auxilium.Core.Services
{
	public class TenantService : ITenantService
	{
		private readonly ICosmosRepository<AzureTenantDto> _tenantRepository;

		public TenantService(ICosmosRepository<AzureTenantDto> tenantRepository)
		{
			_tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
		}
		
		public async Task SaveTenantAsync(IList<AzureTenantDto> tenants)
		{
			await _tenantRepository.UpsertItemsAsync(tenants);
		}

		public async Task<IEnumerable<AzureTenantDto>> GetTenantsAsync()
		{
			return await _tenantRepository.GetItemsAsync<AzureTenantDto>(null, null);
		}
	}
}