/******************************************************************************
 * Filename    = EntityApiFunctions.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = CloudProgrammingDemo
 * 
 * Project     = ServerlessFuncUnitTesting
 *
 * Description = Defines the unit tests for our ServerlessFunc project.
 *****************************************************************************/


using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using ServerlessFunc;

namespace ServerlessFuncUnitTesting
{
    [TestClass]
    public class ServerlessFuncUnitTests
    {
        private const string BaseUrl = @"http://localhost:7074/api/entity";
        private RestClient _restClient;

        public ServerlessFuncUnitTests()
        {
            _restClient = new(BaseUrl);
        }

        /// <summary>
        /// Cleans up the test leftovers.
        /// </summary>
        [TestCleanup]
        public async Task Cleanup()
        {
            // Delete all entries from our Azure table storage.
            Logger.LogMessage("Deleting all entries from our Azure table storage.");
            await _restClient.DeleteEntitiesAsync();
        }

        /// <summary>
        /// Tests creating and getting an entity.
        /// </summary>
        [TestMethod]
        public async Task TestPostAndGet()
        {
            // Create an entity.
            Logger.LogMessage("Create an entity.");
            Entity? postEntity = await _restClient.PostEntityAsync("First");

            // Get the entity.
            Logger.LogMessage("Get the entity.");
            Entity? getEntity = await _restClient.GetEntityAsync(postEntity?.Id);

            // Validate.
            Logger.LogMessage("Validate.");
            Assert.AreEqual(postEntity?.Id, getEntity?.Id);
            Assert.AreEqual(postEntity?.Name, getEntity?.Name);
        }

        /// <summary>
        /// Tests updating an entity.
        /// </summary>
        [TestMethod]
        public async Task TestPut()
        {
            // Create an entity.
            Logger.LogMessage("Create an entity.");
            Entity? postEntity = await _restClient.PostEntityAsync("First");

            // Update the entity.
            Logger.LogMessage("Update the entity.");
            Entity? updatedEntity = await _restClient.PutEntityAsync(postEntity?.Id, "Updated First");

            // Validate.
            Logger.LogMessage("Validate.");
            Entity? getEntity = await _restClient.GetEntityAsync(postEntity?.Id);
            Assert.AreEqual(updatedEntity?.Id, getEntity?.Id);
            Assert.AreEqual(updatedEntity?.Name, getEntity?.Name);
        }

        /// <summary>
        /// Tests deleting all entities and getting all entities.
        /// </summary>
        [TestMethod]
        public async Task TestDeleteAllAndGetAll()
        {
            // Delete any existing entities.
            Logger.LogMessage("Delete any existing entities.");
            await _restClient.DeleteEntitiesAsync();

            // Create three entities.
            Logger.LogMessage("Create three entities.");
            _ = await _restClient.PostEntityAsync("First");
            _ = await _restClient.PostEntityAsync("Second");
            _ = await _restClient.PostEntityAsync("Third");

            // Validate.
            Logger.LogMessage("Validate.");
            IReadOnlyList<Entity>? entities = await _restClient.GetEntitiesAsync();
            Assert.AreEqual(entities?.Count, 3);
        }
    }
}