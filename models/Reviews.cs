using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Restaurant.Models
{
    public class Review
    {   
        [BsonElement("_id")]
        [BsonId]
        [JsonProperty("_id")]
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        [BsonElement("restaurnat_id")]
        [JsonProperty("restaurnat_id")]
        [JsonIgnore]
        public ObjectId RestaurantId { get; set; }
    }
}