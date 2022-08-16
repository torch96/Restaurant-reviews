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
        [BsonElement("_id")]
        [JsonProperty("_id")]

        private List<Review> reviews;
        public string Id { get; set; }
        
        public Address Address { get; set; }
        public Grades Grades { get; set; }

        public string borough { get; set; }
        public string cuisine { get; set; }
        public string name { get; set; }

        public List<Review> Reviews
        {
            get
            {
                return reviews;
            }
            set
            {
                reviews = value;
                if (reviews != null)
                {
                    reviews = reviews.OrderByDescending(r => r.Date).ToList();
                }
            }
        }
        public DateTime LastUpdated { get; set; }
    }

    public class Address
    {
        public string building { get; set; }
        public string coord { get; set; }
        public string street { get; set; }
        public string zipcode { get; set; }
    }

    public class Grades
    {
        public double grade { get; set; }
        public int score { get; set; }
    }
}