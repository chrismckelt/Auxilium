using System;
using System.Collections.Generic;
using System.Text;

namespace Auxilium.Core.LogicApps
{
    public class LogicAppExtract
    {
        public string LogicAppName { get; set; }

        public string ResourceGroup { get; set; }

        public string RunId { get; set; }

        public string Correlation { get; set; }

        public string ActionName { get; set; }

        public string Input { get; set; }

        public string InputLink { get; set; }

        public string Output { get; set; }

        public string OutputLink { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public DateTime? StartTimeUtc { get; set; }

        public DateTime? EndTimeUtc { get; set; }

        public override string ToString()
        {
            return $"{nameof(LogicAppName)}: {LogicAppName}, {nameof(ActionName)}: {ActionName}, {nameof(Input)}: {Input}, {nameof(Output)}: {Output}";
        }
    }
}
