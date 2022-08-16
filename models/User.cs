using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace RestaurantReview.Models
{
    public class User{
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [JsonProperty("auth_token")]
        [BsonIgnore]
        public string AuthToken { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        [JsonIgnore]
        public string HashedPassword { get; set; }

    }

    public class UserLogin{
        [Required]
        [EmailAddress]
        private string Email { get; set; }
        [Required]
        private string Password { get; set; }
    }
}