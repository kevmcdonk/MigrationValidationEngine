using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using Microsoft.Azure.Cosmos;
using Mcd79.MigrationValidationEngine.Core.Model;

namespace Mcd79.MigrationValidationEngine.Core.Services
{
    public class DataRepo
    {
        private string _endpoint;
        private string _primaryKey;
        private string _databaseId;
        private string _containerId;
        public DataRepo(DataRepoConfig config) {
            _endpoint = config.Endpoint;
            _primaryKey = config.PrimaryKey;
            _databaseId = config.DatabaseId;
            _containerId = config.ContainerId;
        }
        public async void Save<T>(T item) {
            using (CosmosClient cosmosClient = new CosmosClient(_endpoint, _primaryKey))
{
                Container container = cosmosClient.GetContainer("DatabaseId", "ContainerId");
                // Read item from container
                ItemResponse<T> todoItemResponse = await container.CreateItemAsync<T>(item);
            }
        }
    }
}
