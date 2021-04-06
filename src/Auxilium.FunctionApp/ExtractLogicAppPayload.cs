using System;
using System.Collections.Generic;
using System.Text;

namespace Auxilium.FunctionApp
{
    public class ExtractLogicAppPayload
    {
        public string ResourceGroup { get; set; }

        public string LogicAppName { get; set; }

        public bool FailedOnly { get; set; } = false;

        public bool Export { get; set; } = true;

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        //string resourceGroupName = req.Query["ResourceGroupName"];
        //string logicAppName = req.Query["LogicAppName"];
        //string failedOnly = req.Query["FailedOnly"];
        //    if (!string.IsNullOrEmpty(failedOnly))
        //{
        //    _extractFailedOnly = Convert.ToBoolean(failedOnly);
        //}

        //string startDateTime = req.Query["StartDateTime"];
        //string endDateTime = req.Query["EndDateTime"];
    }
}