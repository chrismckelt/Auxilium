using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Auxilium.Core.LogicApps
{
    public class LogicAppRunLog : TableEntity
    {
        public LogicAppRunLog()
        {
        }

        public LogicAppRunLog(string resourceId, string rowId)
        {
            PartitionKey = resourceId;
            RowKey = rowId;
        }

        public bool IsActioned { get; set; } = false;
        public bool IsReplayed { get; set; } = false;

        public DateTime? ReplayTime { get; set; }

        public string NewRunId { get; set; }

        public bool? IsReplaySucceed { get; set; } 


    }
}
