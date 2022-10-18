/******************************************************************************
 * Filename    = EntityApiFunctions.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = CloudProgrammingDemo
 * 
 * Project     = ServerlessFunc
 *
 * Description = Defines custom Azure Function Apps APIs.
 *****************************************************************************/

using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerlessFunc
{
    /// <summary>
    /// Custom Azure functions APIs class.
    /// </summary>
    public static class EntityApi
    {
        private const string TableName = "Entities";
        private const string ConnectionName = "AzureWebJobsStorage";
        private const string Route = "entity";

        [FunctionName("CreateEntity")]
        public static async Task<IActionResult> CreateEntity(
                [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route)] HttpRequest req,
                [Table(TableName, Connection = ConnectionName)] IAsyncCollector<Entity> entityTable,
                ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string name = JsonSerializer.Deserialize<string>(requestBody);
            Entity value = new(name);
            await entityTable.AddAsync(value);

            log.LogInformation($"New entity created Id = {value.Id}, Name = {value.Name}.");

            return new OkObjectResult(value);
        }

        [FunctionName("GetEntityById")]
        public static IActionResult GetEntityById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route + "/{id}")] HttpRequest req,
        [Table(TableName, Entity.PartitionKeyName, "{id}", Connection = ConnectionName)] Entity entity,
        ILogger log,
        string id)
        {
            log.LogInformation($"Getting entity {id}");
            if (entity == null)
            {
                log.LogInformation($"Entity {id} not found");
                return new NotFoundResult();
            }

            return new OkObjectResult(entity);
        }

        [FunctionName("GetEntities")]
        public static async Task<IActionResult> GetEntitiesAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route)] HttpRequest req,
        [Table(TableName, Connection = ConnectionName)] TableClient tableClient,
        ILogger log)
        {
            log.LogInformation("Getting all entity items");
            var page = await tableClient.QueryAsync<Entity>().AsPages().FirstAsync();
            return new OkObjectResult(page.Values);
        }

        [FunctionName("UpdateEntity")]
        public static async Task<IActionResult> UpdateEntity(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = Route + "/{id}")] HttpRequest req,
        [Table(TableName, Connection = ConnectionName)] TableClient tableClient,
        ILogger log,
        string id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedName = JsonSerializer.Deserialize<string>(requestBody);
            log.LogInformation($"Updating item with id = {id}");
            Entity existingRow;
            try
            {
                var findResult = await tableClient.GetEntityAsync<Entity>(Entity.PartitionKeyName, id);
                existingRow = findResult.Value;
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new NotFoundResult();
            }

            existingRow.Name = updatedName;
            await tableClient.UpdateEntityAsync(existingRow, existingRow.ETag, TableUpdateMode.Replace);

            return new OkObjectResult(existingRow);
        }

        [FunctionName("DeleteEntity")]
        public static async Task<IActionResult> DeleteEntity(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = Route + "/{id}")] HttpRequest req,
        [Table(TableName, ConnectionName)] TableClient entityClient,
        ILogger log,
        string id)
        {
            log.LogInformation($"Deleting entity by {id}");
            try
            {
                await entityClient.DeleteEntityAsync(Entity.PartitionKeyName, id, ETag.All);
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new NotFoundResult();
            }

            return new OkResult();
        }

        [FunctionName("DeleteEntities")]
        public static async Task<IActionResult> DeleteEntities(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = Route)] HttpRequest req,
        [Table(TableName, ConnectionName)] TableClient entityClient,
        ILogger log)
        {
            log.LogInformation($"Deleting all entity items");
            try
            {
                await entityClient.DeleteAsync();
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new NotFoundResult();
            }

            return new OkResult();
        }
    }
}
