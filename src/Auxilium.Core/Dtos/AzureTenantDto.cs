using System.Collections.Generic;
using Newtonsoft.Json;

namespace Auxilium.Core.Dtos
{
	public class AzureTenantDto : Entity
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("tenantId")]
		public string TenantId { get; set; }

		[JsonProperty("displayName")]
		public string DisplayName { get; set; }

		[JsonProperty("domains")]
		public IList<string> Domains { get; set; }
	}
}