using System.Collections.Generic;
using Newtonsoft.Json;

namespace Auxilium.Core.Workspaces
{
    public class WorkspaceList : IHaveModels<WorkspaceModel>
    {
        [JsonProperty("value")]
        public IList<WorkspaceModel> Value { get; set; }
    }
}