using System;
using System.Text;
using Newtonsoft.Json;

namespace Auxilium.Core.Workspaces
{
    public class Sku
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("maxCapacityReservationLevel")]
        public int MaxCapacityReservationLevel { get; set; }

        [JsonProperty("lastSkuUpdate")]
        public string LastSkuUpdate { get; set; }
    }

    public class Features
    {

        [JsonProperty("legacy")]
        public int Legacy { get; set; }

        [JsonProperty("searchVersion")]
        public int SearchVersion { get; set; }

        [JsonProperty("enableLogAccessUsingOnlyResourcePermissions")]
        public bool EnableLogAccessUsingOnlyResourcePermissions { get; set; }
    }

    public class WorkspaceCapping
    {

        [JsonProperty("dailyQuotaGb")]
        public double DailyQuotaGb { get; set; }

        [JsonProperty("quotaNextResetTime")]
        public string QuotaNextResetTime { get; set; }

        [JsonProperty("dataIngestionStatus")]
        public string DataIngestionStatus { get; set; }
    }

    public class Properties
    {

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("provisioningState")]
        public string ProvisioningState { get; set; }

        [JsonProperty("sku")]
        public Sku Sku { get; set; }

        [JsonProperty("retentionInDays")]
        public int RetentionInDays { get; set; }

        [JsonProperty("features")]
        public Features Features { get; set; }

        [JsonProperty("workspaceCapping")]
        public WorkspaceCapping WorkspaceCapping { get; set; }

        [JsonProperty("publicNetworkAccessForIngestion")]
        public string PublicNetworkAccessForIngestion { get; set; }

        [JsonProperty("publicNetworkAccessForQuery")]
        public string PublicNetworkAccessForQuery { get; set; }

        [JsonProperty("createdDate")]
        public string CreatedDate { get; set; }

        [JsonProperty("modifiedDate")]
        public string ModifiedDate { get; set; }
    }

    public class Tags
    {
    }

    public class WorkspaceModel
    {

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("tags")]
        public Tags Tags { get; set; }
    }
}
