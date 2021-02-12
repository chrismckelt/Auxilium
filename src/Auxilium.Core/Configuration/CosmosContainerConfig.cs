namespace Auxilium.Core.Configuration
{
	public class CosmosContainerConfig
	{
		/// <summary>
		///     Container name
		/// </summary>
		public string ContainerName { get; set; }

		/// <summary>
		///     Partition key
		/// </summary>
		public string PartitionKey { get; set; }
	}
}