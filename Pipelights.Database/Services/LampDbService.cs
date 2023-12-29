using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Pipelights.Database.Models;

namespace Pipelights.Database.Services
{
    public interface ILampDbService
    {
        Task<IEnumerable<ProductEntity>> GetMultipleAsync(string query);
        Task<ProductEntity> GetAsync(string id);
        Task AddAsync(ProductEntity item);
        Task UpdateAsync(ProductEntity item);
        Task DeleteAsync(string id);
    }

    public class LampDbService : ILampDbService
    {
        private Container _container;
        public LampDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }
        public async Task AddAsync(ProductEntity item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.id));
        }
        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<ProductEntity>(id, new PartitionKey(id));
        }
        public async Task<ProductEntity> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<ProductEntity>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) //For handling item not found and other exceptions
            {
                return null;
            }
        }
        public async Task<IEnumerable<ProductEntity>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<ProductEntity>(new QueryDefinition(queryString));
            var results = new List<ProductEntity>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        public async Task UpdateAsync(ProductEntity item)
        {
            await _container.UpsertItemAsync(item);
        }
    }
}
