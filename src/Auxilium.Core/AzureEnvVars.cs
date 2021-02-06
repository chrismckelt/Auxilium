using System;
using System.Collections.Generic;
using System.Text;

namespace Auxilium.Core
{
    public class AzureEnvVars
    {
        public static string ClientId => TryTargets("AZURE_CLIENT_ID");
        public static string TenantId => TryTargets("AZURE_TENANT_ID");

        public static string Username => TryTargets("AZURE_USERNAME");

        public static string Password => TryTargets("AZURE_PASSWORD");

        public static string ClientSecret => TryTargets("AZURE_CLIENT_SECRET");

        public static string WorkspaceId => TryTargets("AZURE_WORKSPACE_ID");

        public static string WorkspaceSharedKey => TryTargets("AZURE_WORKSPACE_SHARED_KEY");

        public static string SubscriptionId => TryTargets("AZURE_SUBSCIPTION_ID");

        public static string SubscriptionName => TryTargets("AZURE_SUBSCIPTION_NAME");

        private static string TryTargets(string name)
        {
            var res = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
            if (!string.IsNullOrEmpty(res)) return res;

            res = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            if (!string.IsNullOrEmpty(res)) return res;

            res = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrEmpty(res)) return res;

            return null;
        }
        /*
         *
         *
         * Environment.SetEnvironmentVariable("AZURE_WORKSPACE_ID", "", EnvironmentVariableTarget.User);
	Environment.SetEnvironmentVariable("AZURE_WORKSPACE_SHARED_KEY", "", EnvironmentVariableTarget.User);
	Environment.SetEnvironmentVariable("AZURE_SUBSCIPTION_ID", "", EnvironmentVariableTarget.User);	
	Environment.SetEnvironmentVariable("AZURE_SUBSCIPTION_NAME", "", EnvironmentVariableTarget.User);

         * *
         */
    }
}
