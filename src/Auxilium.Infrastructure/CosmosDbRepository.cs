using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Auxilium.Core.Configuration;
using Auxilium.Core.Dtos;
using Auxilium.Core.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Auxilium.Infrastructure
{
    public abstract class CosmosDbRepository<T> : ICosmosRepository<T> where T : Entity
    {
        private readonly ILogger<CosmosDbRepository<T>> _logger;
        private readonly CosmosContainerConfig _containerConfig;
        private bool _isInitialized;
        private CosmosClient _client;
        private readonly CosmosConfig _config;
        protected Container Container { get; set; }
        public CosmosConfig Config => _config;
        public Database Database { get; private set; }
        private Container ContainerProxy => _client.GetContainer(_config.DatabaseName, _containerConfig.ContainerName);

        protected CosmosDbRepository(IOptions<CosmosConfig> config, ILogger<CosmosDbRepository<T>> logger,
            CosmosContainerConfig containerConfig)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _containerConfig = containerConfig ?? throw new ArgumentNullException(nameof(containerConfig));
            _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
            InitializeClient();
        }

        private void InitializeClient()
        {
            _client = new CosmosClient(_config.ConnectionString, new CosmosClientOptions { ConnectionMode = ConnectionMode.Direct });
        }

        private async Task EnsureDatabaseAndCollectionExistsAsync()
        {
            if (!_isInitialized)
            {
                try
                {
                    Database = _client.GetDatabase(Config.DatabaseName);
                }
                catch (Exception exception)
                {
                    _logger.LogError($"GetDatabaseAsync: {exception.Message}");
                    throw;
                }

                try
                {
                    var containerProperties = new ContainerProperties
                    {
                        Id = _containerConfig.ContainerName,
                        PartitionKeyPath = $"/{_containerConfig.PartitionKey}"

                    };
                    UpdateContainerProperties(containerProperties);
                    Container = await Database.CreateContainerIfNotExistsAsync(containerProperties);
                }
                catch (Exception exception)
                {
                    _logger.LogError($"EnsureContainerAsync: {exception.Message}");
                    throw;
                }
                int? currentContainerThroughput = await Container.ReadThroughputAsync();
                _logger.LogInformation($"Using container {_containerConfig.ContainerName} with {currentContainerThroughput} RU/s");
                _isInitialized = true;
            }
        }

        public async Task UpsertItemsAsync(IList<T> items)
        {
            List<double> totalRU = new List<double>();
            await EnsureDatabaseAndCollectionExistsAsync().ConfigureAwait(false);
            foreach (var item in items)
            {
                var response = await Container.UpsertItemAsync(item);
                totalRU.Add(response.RequestCharge);
                if (!IsSuccessStatusCode(response.StatusCode))
                {
                    _logger.LogInformation($"Received {response.StatusCode} ({item.Name}).");
                }
            }
            _logger.LogInformation($"RU Sum: {totalRU.Sum()}, Avg: {totalRU.Average()}");
        }

        public static bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            return ((int)statusCode >= 200) && ((int)statusCode <= 299);
        }

        public async Task UpsertItemsBatchAsync(IList<T> items)
        {
            var itemsToInsert = new List<Tuple<PartitionKey, Stream>>();
            
            foreach (var item in items)
            {
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(item)));
                itemsToInsert.Add(new Tuple<PartitionKey, Stream>(new PartitionKey(item.PartitionKey), stream));
            }

            try
            {
                await EnsureDatabaseAndCollectionExistsAsync().ConfigureAwait(false);

                var tasks = new List<Task>();

                foreach (Tuple<PartitionKey, Stream> item in itemsToInsert)
                {
                    tasks.Add(Container.UpsertItemStreamAsync(item.Item2, item.Item1)
                        .ContinueWith((Task<ResponseMessage> task) => {
                            using (ResponseMessage response = task.Result)
                            {
                                if (!response.IsSuccessStatusCode)
                                {
                                    _logger.LogInformation($"Received {response.StatusCode} ({response.ErrorMessage}).");
                                }
                            }
                        }));
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Upsert", items.Count.ToString());
                throw;
            }
        }

        /// <summary>
        ///     Get items 
        /// </summary>
        /// <typeparam name="T">the item type</typeparam>
        /// <param name="predicate">The predicate to filter results</param>
        /// <param name="projection">The projected fields in results</param>
        /// <param name="orderBy">The order by predicate to sort results</param>
        /// <param name="ascending">Sort results in ascending or descending order</param>
        /// <param name="limit">Limit results</param>
        /// <param name="firstPageOnly">Get the first page only</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, T>> projection = null,
            Expression<Func<T, T>> orderBy = null,
            bool ascending = true,
            int limit = 0,
            bool firstPageOnly = false)
        {
            try
            {
                var results = new List<T>();

                IQueryable<T> queryable = ContainerProxy.GetItemLinqQueryable<T>();

                if (predicate != null)
                {
                    queryable = queryable.Where(predicate);
                }

                if (orderBy != null)
                {
                    queryable = ascending ? queryable.OrderBy(orderBy) : queryable.OrderByDescending(orderBy);
                }

                if (limit > 0)
                {
                    queryable = queryable.Take(limit);
                }

                if (projection != null)
                {
                    queryable = queryable.Select(projection);
                }

                FeedIterator<T> feedIterator = queryable.ToFeedIterator();

                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<T> feedResponse = await feedIterator.ReadNextAsync().ConfigureAwait(false);

                    if (firstPageOnly)
                    {
                        return feedResponse;
                    }

                    results.AddRange(feedResponse);
                }

                return results;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetItemsAsync");
                throw;
            }
        }

        public virtual void UpdateContainerProperties(ContainerProperties props) { }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate = default, Expression<Func<T, T>> projection = default)
        {
            return await GetItemsAsync<T>(predicate, projection, null);
        }
    }
}