using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Pipelights.Database.Models;

namespace Pipelights.Database.Services
{
    public interface ITestimonialDbService
    {
        Task<IEnumerable<TestimonialEntity>> GetMultipleAsync(string query);
        Task<TestimonialEntity> GetAsync(string id);
        Task AddAsync(TestimonialEntity item);
        Task UpdateAsync(TestimonialEntity item);
        Task DeleteAsync(string id);
    }

    public class TestimonialDbService : ITestimonialDbService
    {
        private Container _container;
        public TestimonialDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }
        public async Task AddAsync(TestimonialEntity item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.id));
        }
        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<TestimonialEntity>(id, new PartitionKey(id));
        }


        public async Task<TestimonialEntity> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<TestimonialEntity>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) //For handling item not found and other exceptions
            {
                return null;
            }
        }
        public async Task<IEnumerable<TestimonialEntity>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<TestimonialEntity>(new QueryDefinition(queryString));
            var results = new List<TestimonialEntity>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
        public async Task UpdateAsync(TestimonialEntity item)
        {
            await _container.UpsertItemAsync(item);
        }
    }
}
