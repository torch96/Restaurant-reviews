using MongoDB.Bson.Serialization.Attributes;

namespace Restaurant.Models
{
    public class session
    {
        [BsonElement("user_id")]
        public string UserId { get; set; }
        
        public string Jwt { get; set; }
    }
}