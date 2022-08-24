using System.Collections.Generic;
using System.Dynamic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestaurantReview.Models.Projections
{
    
        public class RestaurantByTextProjection : Restaurant
        {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

       
        }
    
}