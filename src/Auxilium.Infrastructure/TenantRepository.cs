using Auxilium.Core.Configuration;
using Auxilium.Core.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Auxilium.Infrastructure
{
	public class TenantRepository : CosmosDbRepository<AzureTenantDto>
	{
		private static readonly CosmosContainerConfig _tenantContainerConfig = new CosmosContainerConfig
		{
			ContainerName = "Tenants",
			PartitionKey = "tenantId"
		};
		
		public TenantRepository(IOptions<CosmosConfig> config, ILogger<CosmosDbRepository<AzureTenantDto>> logger) : base(config, logger, _tenantContainerConfig)
		{
		}
	}
}