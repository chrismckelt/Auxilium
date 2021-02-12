using Newtonsoft.Json;

namespace Auxilium.Core.Dtos
{
	public class Entity
	{
		public string PartitionKey { get; set; }

		/// <summary>
		///     Name of the item
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     ETag of the item
		/// </summary>
		[JsonProperty(PropertyName = "_etag")]
		public string ETag { get; set; }
        
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}