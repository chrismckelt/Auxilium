using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Auxilium.Core.Resources
{

    public class Sku
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("tier")] public string Tier { get; set; }

        [JsonProperty("capacity")] public int? Capacity { get; set; }

        [JsonProperty("family")] public string Family { get; set; }
    }

    public class Plan
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("promotionCode")] public string PromotionCode { get; set; }

        [JsonProperty("product")] public string Product { get; set; }

        [JsonProperty("publisher")] public string Publisher { get; set; }
    }

    public class Identity
    {
        [JsonProperty("principalId")] public string PrincipalId { get; set; }

        [JsonProperty("tenantId")] public string TenantId { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("userAssignedIdentities")]
        public dynamic UserAssignedIdentities { get; set; }
    }

    public class Resource
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("location")] public string Location { get; set; }

        [JsonProperty("tags")] public Dictionary<string, string> Tags { get; set; }

        [JsonProperty("sku")] public Sku Sku { get; set; }

        [JsonProperty("kind")] public string Kind { get; set; }

        [JsonProperty("managedBy")] public string ManagedBy { get; set; }

        [JsonProperty("plan")] public Plan Plan { get; set; }

        [JsonProperty("identity")] public Identity Identity { get; set; }
    }

    public class ResourceList : IHaveModels<Resource>
    {
        [JsonProperty("value")] public IList<Resource> Value { get; set; }
    }
}