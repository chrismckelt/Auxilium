using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auxilium.Core.LogicApps
{
   
    public interface ILogicAppService : IHaveAToken
    {
        int PageCountToRetrieve { get; set; } // number of pages history to go back over the logic app run
        LogicAppRunFilter Filter { get; set; }
        string LogicAppName { get; set; }
        IList<LogicAppWorkflowRun> WorkflowRuns { get; }
        Task<AzureLogicApps> ListAsync(string subscriptionId, string resourceGroupName);

        Task<AzureLogicAppWorkflowRuns> WorkflowRunListAsync(string subscriptionId,
            string resourceGroupName, string logicAppName, DateTime? startTimeBegin = new DateTime?(),
            DateTime? startTimeEnd = new DateTime?());

        Task<LogicAppRunAction> WorkflowRunGetActionAsync(string subscriptionId,
            string resourceGroupName, string logicAppName, string runId, string actionName);

        Task<AzureLogicAppWorkflowRuns> WorkflowRunGetAsync(string subscriptionId,
            string resourceGroupName, string workflowName, string runName);

        Task<AzureLogicAppWorkflowTriggers> WorkflowTriggersListAsync(
            string subscriptionId, string resourceGroupName, string workflowName);

        Task<AzureLogicAppWorkflowTriggerHistoryRun> WorkflowTriggerHistoryListAsync(string subscriptionId,
            string trigger,
            string resourceGroupName, string logicAppName);

        Task<LogicAppResubmittedRun> ResubmitAsync(string subscriptionId,
            string resourceGroupName, string logicAppName, string triggerHistoryRunId, string triggerName = "manual");

        Task<IEnumerable<LogicAppWorkflowRun>> FetchContentAsync(string subscriptionId, string resourceGroupName,
            string logicAppName, AzureLogicAppWorkflowRuns runs);
    }
}
