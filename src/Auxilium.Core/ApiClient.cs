using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Rest;
using Auxilium.Core.LogAnalytics;
using Auxilium.Core.LogicApps;
using Auxilium.Core.ResourceGroups;
using Auxilium.Core.Resources;
using Auxilium.Core.Storage;
using Auxilium.Core.Tenants;
using Auxilium.Core.Utilities;
using Auxilium.Core.Workspaces;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;


namespace Auxilium.Core
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme
    /// </summary>
    public class ApiClient : IApiClient
    {
        public string SubscriptionId { get; }

        public IAzure AzureService { get; private set; }
        public string Token { get; private set; }
        public string TenantId { get; private set; }

        public ITenantService TenantService { get; set; } = new TenantService();
        public IResourceGroupService ResourceGroupService { get; set; } = new ResourceGroupService();
        public IResourceService ResourceService { get; set; } = new ResourceService();
        public ILogicAppService LogicAppService { get; set; } = new LogicAppService();
        public IWorkspacesService WorkspacesService { get; set; } = new WorkspacesService();

        public ILogAnalyticsService LogAnalyticsService { get; set; } = new LogAnalyticsService();

        public ILogAnalyticsDataCollector LogAnalyticsDataCollector { get; set; } = new LogAnalyticsDataCollector(AzureEnvVars.WorkspaceId, AzureEnvVars.WorkspaceSharedKey);

        public T Create<T>() where T : IHaveAToken
        {
            var t = (IHaveAToken)Activator.CreateInstance(typeof(T),Token);

            return (T)t;
        }

        public ApiClient(string tenantId, string subscriptionId, string token= null)
        {
            TenantId = tenantId;
            SubscriptionId = subscriptionId;
            if (!string.IsNullOrEmpty(token))
            {
                Token = token;
                Init();
                SetAccessToken(token);
            }
        }

        public void Init()
        {
            if (AzureService != null) return;

            var tokenCredentials = new TokenCredentials(Token);
            var azureCredentials = new AzureCredentials(
                tokenCredentials,
                tokenCredentials,
                TenantId,
                AzureEnvironment.AzureGlobalCloud);
            var client = RestClient
                .Configure()
                .WithEnvironment(AzureEnvironment.AzureGlobalCloud)
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .WithCredentials(azureCredentials)
                .Build();

            try
            {
                AzureService = Microsoft.Azure.Management.Fluent.Azure.Authenticate(client, TenantId).WithSubscription(subscriptionId: SubscriptionId);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static string GetToken()
        {
            return AuthUtil.GetTokenFromConsole();
            https://yourazurecoach.com/2020/08/13/managed-identity-simplified-with-the-new-azure-net-sdks/
            var options = new DefaultAzureCredentialOptions
            {
                ExcludeEnvironmentCredential = false,
                ExcludeManagedIdentityCredential = false,
                ExcludeSharedTokenCacheCredential = false,
                ExcludeVisualStudioCredential = true,
                ExcludeVisualStudioCodeCredential = true,
                ExcludeAzureCliCredential = false,
                ExcludeInteractiveBrowserCredential = false
            };
            var credential = new DefaultAzureCredential(options);
            //var credential = new ChainedTokenCredential(new AzureCliCredential(), new ManagedIdentityCredential());
            var token = credential.GetToken(
                new Azure.Core.TokenRequestContext(
                    new[] { "https://graph.microsoft.com/.default" }));

            var accessToken = token.Token;
            //var graphServiceClient = new GraphServiceClient(
            //    new DelegateAuthenticationProvider((requestMessage) =>
            //    {
            //        requestMessage
            //            .Headers
            //            .Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            //        return Task.CompletedTask;
            //    }));

            return accessToken;
        }

        public void SetAccessToken(string token)
        {
            Token = token;
            TenantService = new TenantService(Token);
            ResourceGroupService = new ResourceGroupService(Token);
            ResourceService = new ResourceService(Token);
            LogicAppService = new LogicAppService(Token);
            WorkspacesService = new WorkspacesService(Token);
            LogAnalyticsService = new LogAnalyticsService(Token);
            Init();
        }

        #region storage

        public async Task LogToStorageAccount(IEnumerable<LogicAppWorkflowRun> logicAppWorkflowRuns, string storageAccountConnectionString, string tableName)
        {
            //string storageAccountConnectionString = "DefaultEndpointsProtocol=https;AccountName=azaust1lam585;AccountKey=1xTEw9Zf4aUtjfEaJs0DUNm5UWJ8t9mELjBOt+COd2dzfdngOr2uiahMJpG/8z7GegmzZ5RoA2mAbqS9XM9fvg==;EndpointSuffix=core.windows.net";
            //string tableName = "LogicAppRunHistory";

            AzureStorageService azureStorageService = new AzureStorageService(storageAccountConnectionString);
            var createTable = await azureStorageService.CreateCloudTable(tableName);

            foreach (var item in logicAppWorkflowRuns)
            {
                LogicAppRunLog entity = new LogicAppRunLog();
                entity.PartitionKey = $"{item.SubscriptionId}@{item.ResourceGroupName}@{item.LogicAppName}";
                entity.RowKey = $"{item.Name}";

                var existedRecord = await azureStorageService.RetrieveEntityFromTable<LogicAppRunLog>(tableName, partitionKey: entity.PartitionKey, rowKey: entity.RowKey);

                if (existedRecord == null)
                {
                    await azureStorageService.InsertOrMergeEntityToTable(tableName, entity, enableMerge: false);
                }
            }

        }

        public async Task UpdateStorageAccountReplayLog(string storageAccountConnectionString, string tableName, string subscriptionId, string resourceGroupName, string logicAppName, string oldRunId, string newRunId)
        {
            AzureStorageService azureStorageService = new AzureStorageService(storageAccountConnectionString);
            var _createTable = await azureStorageService.CreateCloudTable(tableName);

            LogicAppRunLog entity = new LogicAppRunLog();
            entity.PartitionKey = $"{subscriptionId}@{resourceGroupName}@{logicAppName}";
            entity.RowKey = $"{oldRunId}";

            entity.IsReplayed = true;
            entity.NewRunId = newRunId;

            await azureStorageService.InsertOrMergeEntityToTable(tableName, entity, enableMerge: true);



        }

        #endregion

    }
}