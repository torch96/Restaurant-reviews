using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using RestaurantReview.Models.Projections;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
namespace RestaurantReview.Models.Responses
{
    public class RestaurantResponse
    {
        private static readonly int  RESTAURANTS_PER_PAGE = 10;
        
        public RestaurantResponse(Restaurant restaurant)
        {
           if(Restaurant  != null)
           {
               Restaurant = restaurant;
               Api = "csharp";
               UpdatedType = (restaurant.LastUpdated is DateTime) ? "Date" : "Other";
           }
        }
        public RestaurantResponse(IReadOnlyList<Restaurant> restaurants, long totalRestaurantCount, int page, Dictionary<string, object> filters)
        {
            Restaurants = restaurants;
            RestaurantsCount = totalRestaurantCount;
            EntriesPerPage = RESTAURANTS_PER_PAGE;
            Page = page;
            Filters = filters ?? new Dictionary<string, object>();
        }

    
        public RestaurantResponse(IReadOnlyList<RestaurantByTextProjection> restaurants, long totalRestaurantCount, int page, Dictionary<string, object> filters)
        {
            Restaurants = restaurants;
            RestaurantsCount = totalRestaurantCount;
            EntriesPerPage = RESTAURANTS_PER_PAGE;
            Page = page;
            Filters = filters ?? new Dictionary<string, object>();
        }

        [JsonProperty("Restaurant", NullValueHandling = NullValueHandling.Ignore)]
        public Restaurant Restaurant { get; set; }

        [JsonProperty("Restaurants", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<Restaurant> Restaurants { get; set; }
        [JsonProperty("RestaurantsCount", NullValueHandling = NullValueHandling.Ignore)]
        public long RestaurantsCount { get; set; }
        [JsonProperty("EntriesPerPage", NullValueHandling = NullValueHandling.Ignore)]
        public int EntriesPerPage { get; set; }
        [JsonProperty("Page", NullValueHandling = NullValueHandling.Ignore)]
        public int Page { get; set; }
        [JsonProperty("Filters", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Filters { get; set; }
        [JsonProperty("Api", NullValueHandling = NullValueHandling.Ignore)]
        public string Api { get; set; }
        [JsonProperty("UpdatedType", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedType { get; set; }
        [JsonProperty("LastUpdated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdated { get; set; }

        
    }
}