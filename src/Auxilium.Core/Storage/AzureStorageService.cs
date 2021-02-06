using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace Auxilium.Core.Storage
{
    public class AzureStorageService
    {
        private StorageCredentials _storageCredentials;
        private CloudStorageAccount _storageAccount;

        public AzureStorageService(string storageAccountConnectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
        }

        public CloudTable GetCloudTable(string tableName)
        {
            // Create the table client.
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

            CloudTable cloudTable = tableClient.GetTableReference(tableName);

            return cloudTable;
        }

        public async Task<bool> CreateCloudTable(string tableName)
        {
            try
            {
                CloudTable cloudTable = GetCloudTable(tableName);

                return await cloudTable.CreateIfNotExistsAsync();
            }
            catch (Exception ee)
            {
                Console.WriteLine($"Create table failed: {ee.Message}");
            }

            return false;

        }

        public async Task<TableResult> InsertOrMergeEntityToTable(string tableName, ITableEntity entity, bool enableMerge = false)
        {
            CloudTable cloudTable = GetCloudTable(tableName);

            if (entity != null)
            {
                try
                {
                    TableOperation tableOperation;
                    if (enableMerge)
                    {
                        tableOperation = TableOperation.InsertOrMerge(entity);
                    }
                    else
                    {
                        tableOperation = TableOperation.Insert(entity);
                    }
                    var _task = await cloudTable.ExecuteAsync(tableOperation);
                    Console.WriteLine("Record inserted/updated");
                    return _task;
                }
                catch (Exception ee)
                {
                    Console.WriteLine($"Insert/Update record failed: {ee.Message}");
                }
            }
            else
            {
                Console.WriteLine("Record is null");
            }

            return null;
        }


        public async Task<T> RetrieveEntityFromTable<T>(string tableName, string partitionKey, string rowKey) where T : ITableEntity
        {
            var cloudTable = GetCloudTable(tableName);

            try
            {
                var tableOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                var task = await cloudTable.ExecuteAsync(tableOperation);
                //Console.WriteLine("Table record retrieved");

                var result = (T)task.Result;
                return result;
            }
            catch (Exception ee)
            {
                Console.WriteLine($"Retrieve table record failed: {ee.Message}");
            }

            return default(T);
        }

        public async Task<TableResult> DeleteEntityFromTable(string tableName, ITableEntity entity)
        {
            CloudTable cloudTable = GetCloudTable(tableName);

            if (entity != null)
            {
                try
                {
                    TableOperation tableOperation = TableOperation.Delete(entity);
                    var _task = await cloudTable.ExecuteAsync(tableOperation);
                    Console.WriteLine("Table record deleted");
                    return _task;
                }
                catch (Exception ee)
                {
                    Console.WriteLine($"DeleteLogAlert table record failed: {ee.Message}");
                }
            }
            else
            {
                Console.WriteLine("Record is null");
            }

            return null;
        }


        public async Task<bool> DropCloudTable(string tableName)
        {
            CloudTable cloudTable = GetCloudTable(tableName);

            return await cloudTable.DeleteIfExistsAsync();
            
        }
    }
}
