/******************************************************************************
 * Filename    = EntityApiFunctions.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = CloudProgrammingDemo
 * 
 * Project     = ServerlessFuncUnitTesting
 *
 * Description = Defines the Helper class for calling our REST APIs.
 *****************************************************************************/

using ServerlessFunc;
using System.Net.Http.Json;
using System.Text.Json;

namespace ServerlessFuncUnitTesting
{
    /// <summary>
    /// Helper class for calling our REST APIs.
    /// </summary>
    internal class RestClient
    {
        private readonly HttpClient _entityClient;
        private readonly string _url;

        /// <summary>
        /// Creates an instance of the RestClient class.
        /// </summary>
        /// <param name="url">The base URL of the http client.</param>
        public RestClient(string url)
        {
            _entityClient = new();
            _url = url;
        }

        /// <summary>
        /// Makes a "GET" call to our Azure function APIs to get a particular entity.
        /// </summary>
        /// <param name="id">The id of the entity to get.</param>
        /// <returns>The entity with the given id.</returns>
        public async Task<Entity?> GetEntityAsync(string? id)
        {
            var response = await _entityClient.GetAsync(_url + $"/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };

            Entity? entity = JsonSerializer.Deserialize<Entity>(result, options);
            return entity;
        }

        /// <summary>
        /// Makes a "GET" call to our Azure function APIs to get all entities.
        /// </summary>
        /// <returns>All the entities created so far</returns>
        public async Task<IReadOnlyList<Entity>?> GetEntitiesAsync()
        {
            var response = await _entityClient.GetAsync(_url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };

            IReadOnlyList<Entity>? entities = JsonSerializer.Deserialize<IReadOnlyList<Entity>>(result, options);
            return entities;
        }

        /// <summary>
        /// Makes a "PUT" call to our Azure function APIs to update an entity.
        /// </summary>
        /// <param name="id">The Id of the entity to be updated.</param>
        /// <param name="newName">The new name of the entity.</param>
        /// <returns>The updated entity.</returns>
        public async Task<Entity?> PutEntityAsync(string? id, string newName)
        {
            using HttpResponseMessage response = await _entityClient.PutAsJsonAsync<string>(_url + $"/{id}", newName);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };

            Entity? entity = JsonSerializer.Deserialize<Entity>(result, options);
            return entity;
        }

        /// <summary>
        /// Makes a "POST" call to our Azure function APIs.
        /// </summary>
        /// <param name="name">Name of the entity.</param>
        /// <returns>A new entity with the given name.</returns>
        public async Task<Entity?> PostEntityAsync(string name)
        {
            using HttpResponseMessage response = await _entityClient.PostAsJsonAsync<string>(_url, name);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };

            Entity? entity = JsonSerializer.Deserialize<Entity>(result, options);
            return entity;
        }

        /// <summary>
        /// Makes a "DELETE" call to our Azure function APIs to delete a particular entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Task.</returns>
        public async Task DeleteEntityAsync(string? id)
        {
            using HttpResponseMessage response = await _entityClient.DeleteAsync(_url + $"/{id}");
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Makes a "DELETE" call to our Azure function APIs to delete all entities.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task DeleteEntitiesAsync()
        {
            using HttpResponseMessage response = await _entityClient.DeleteAsync(_url);
            response.EnsureSuccessStatusCode();
        }
    }
}
