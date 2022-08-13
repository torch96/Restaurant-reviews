using MongoDB.Bson.Serialization.Attributes;

namespace Restaurant.Models
{
    public class Restaurant
    {
        [BsonElement("_id")]
        [JsonProperty("_id")]
        public string Id { get; set; }
        
        public Address Address { get; set; }
        public Grades Grades { get; set; }

        public string borough { get; set; }
        public string cuisine { get; set; }
        public string name { get; set; }

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