using System;
using System.Collections.Generic;
using System.Text;
using Auxilium.Core.Utilities;

namespace Auxilium.Core
{
    public class AzureEnvVars
    {
        public static string ClientId => TryTargets("AZURE_CLIENT_ID"); // 1950a258-227b-4e31-a9cf-717495945fc2 --> pre-defined client from Microsoft for public use
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


            Consoler.Warn("Env var not found", name);

            return null;
        }


        public static void Print()
        {
            Console.WriteLine(TryTargets(AzureEnvVars.TenantId));

            AzureEnvVars.TenantId.Dump("TenantId");
            AzureEnvVars.SubscriptionId.Dump("SubscriptionId");
            AzureEnvVars.SubscriptionName.Dump("SubscriptionName");
            AzureEnvVars.ClientId.Dump("ClientId");
            AzureEnvVars.ClientSecret.Dump("ClientSecret");
            AzureEnvVars.Username.Dump("Username");
            AzureEnvVars.Password.Dump("Password");

            AzureEnvVars.WorkspaceId.Dump("WorkspaceId");
            AzureEnvVars.WorkspaceSharedKey.Dump("WorkspaceSharedKey");

          
        

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
