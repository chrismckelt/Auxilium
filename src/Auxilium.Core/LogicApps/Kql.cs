using System;
using System.Collections.Generic;
using System.Text;

namespace Auxilium.Core.LogicApps
{
    public static class Kql
    {
        public const string Search = @"AzureDiagnostics
| where ResourceProvider == 'MICROSOFT.LOGIC'
| where Category == 'WorkflowRuntime'
| where status_s == 'Failed'
| project ResourceGroup, resource_workflowName_s, resource_runId_s
| distinct ResourceGroup, resource_workflowName_s, resource_runId_s
| order by resource_workflowName_s";

        public const string ApplicationLog = @"ApplicationLog_CL 
| where TimeGenerated > ago(24h) 
//| where RunId_s == ""08585891302710426929200964427CU14""
//| where Output_s contains ""502099126"" or Input_s contains ""502099126""
| extend j=parse_json(Input_s),e=parse_json(Output_s)
//| project StartTimeUtc_t, SalesOrderId=j[""body""][""transactionId""],LogicAppName_s, Status_s, ActionName_s, StatusCode_s, Error=e[""body""][""message""],Input_s, Output_s
//| where isnotnull(SalesOrderId )
//| where SalesOrderId == ""502099126"" 
| limit 1000";
    }
}