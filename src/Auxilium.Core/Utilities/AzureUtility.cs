using System;

namespace Auxilium.Core.Utilities
{
    public class AzureUtility
    {
        public static string ParseResourceGroupName(string id)
        {
            //"/subscriptions/4d123af1-c6dc-488f-8979-57f6607a585b/resourcegroups/blog/providers/microsoft.operationalinsights/workspaces/
            //Get the string position of the first word and add two (for it's length)
            int pos1 = id.IndexOf("resourcegroups", StringComparison.Ordinal) + 2;

            //Get the string position of the next word, starting index being after the first position
            int pos2 = id.IndexOf("providers", pos1, StringComparison.Ordinal);

            //use substring to obtain the information in between and store in a new string
            string data = id.Substring(pos1, pos2 - pos1).Trim();
            data = data.Replace("sourcegroups", "");
            return data.Replace("/", "");
        }

        public static int MapSeverity(string severityName)
        {
            if (string.IsNullOrEmpty(severityName)) return 1;

            switch (severityName.ToLowerInvariant())
            {
                case "critical":
                    return 1;
                case "error":
                    return 2;
                case "warning":
                    return 3;
                case "information":
                    return 4;
                default:
                    return 0;
            }
        }
    }
}
