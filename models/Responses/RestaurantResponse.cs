using Newtonsoft.Json;

namespace Restaurant.Models.Responses
{
    public class RestaurantResponse
    {
        private static readonly int  RESTAURANTS_PER_PAGE = 10;
        
        public RestaurantResponse(Restaurant restaurant)
        {
            if(restaurant == null)
            {
                Success = false;
                ErrorMessage = "Restaurant not found";
            }
            else
            {
                Success = true;
                Restaurant = restaurant;
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

        public RestaurantResponse(IReadOnlyList<RestaurantByCountryProjection> restaurantsByCountry, long totalRestaurantCount, int page, Dictionary<string, object> filters)
        {
            Restaurants = restaurantsByCountry.Select(x => new KeyValuePair<ObjectId, string>(x.Id, x.Name)).ToList();
            EntriesPerPage = RESTAURANTS_PER_PAGE;
            RestaurantsCount = totalRestaurantCount;
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

        [JsonProperty("restaurant", NullValueHandling = NullValueHandling.Ignore)]
        public Restaurant Restaurant { get; set; }

        [JsonProperty("restaurants", NullValueHandling = NullValueHandling.Ignore)]
        
    }
}