/******************************************************************************
 * Filename    = Entity.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = CloudProgrammingDemo
 * 
 * Project     = ServerlessFunc
 *
 * Description = Defines a custom Azure Table Entity.
 *****************************************************************************/

using Azure;
using System;
using System.Text.Json.Serialization;
using ITableEntity = Azure.Data.Tables.ITableEntity;

namespace ServerlessFunc
{
    /// <summary>
    /// Custom Azure Table Entity.
    /// </summary>
    public class Entity : ITableEntity
    {
        public const string PartitionKeyName = "EntityPartitionKey";

        public Entity(string name)
        {
            PartitionKey = PartitionKeyName;
            RowKey = Guid.NewGuid().ToString();
            Id = RowKey;
            Name = name;
            Timestamp = DateTime.Now;
        }

        public Entity() : this(null) { }

        [JsonInclude]
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonInclude]
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonInclude]
        [JsonPropertyName("PartitionKey")]
        public string PartitionKey { get; set; }

        [JsonInclude]
        [JsonPropertyName("RowKey")]
        public string RowKey { get; set; }

        [JsonInclude]
        [JsonPropertyName("Timestamp")]
        public DateTimeOffset? Timestamp { get; set; }

        [JsonIgnore]
        public ETag ETag { get; set; }
    }
}
