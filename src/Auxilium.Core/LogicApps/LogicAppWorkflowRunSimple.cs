using System;

namespace Auxilium.Core.LogicApps
{
    public class LogicAppWorkflowRunSimple
    {
        public virtual string LogicAppName { get; set; }
        public virtual string TriggerName { get; set; }
        public virtual string RunId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime StartTimeUtc { get; set; }
        public virtual DateTime EndTimeUtc { get; set; }

    }
}
