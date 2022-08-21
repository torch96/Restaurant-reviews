using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using System;
using Newtonsoft.Json;

namespace RestaurantReview.Models
{
    public class Restaurant
    {   
        
        
        private List<Review> reviews;
        private string _id;

        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        
        public string Id { 
            get { return this._id; }
            set { this._id = value; }
        }
        
        public Address address { get; set; }
        public List<Grades> grades { get; set; }

        public string borough { get; set; }
        public string cuisine { get; set; }
        public string name { get; set; }

        public List<Review> Reviews
        {
            get { return reviews != null ? reviews.OrderByDescending(c => c.Date).ToList() : new List<Review>(); }
            set => reviews = value;
        }

        public string restaurant_id { get; set; }
        
        public DateTime LastUpdated { get; set; }
    }

    public class Address
    {
        public string building { get; set; }
        public List<double> coord { get; set; }
        public string street { get; set; }
        public string zipcode { get; set; }
    }

    public class Grades
    {   
        public DateTime date { get; set; }
        public string grade { get; set; }
        public int score { get; set; }
    }
}