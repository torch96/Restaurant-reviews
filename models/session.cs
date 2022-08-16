using MongoDB.Bson.Serialization.Attributes;

namespace RestaurantReview.Models
{
    public class Session
    {
        [BsonElement("user_id")]
        public string UserId { get; set; }
        
        public string Jwt { get; set; }
    }
}