using System;
using System.Threading;
using System.Threading.Tasks;
using RestaurantReview.Models;
using RestaurantReview.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RestaurantReview.Models.Responses;
namespace RestaurantReview.Controllers
{


    public class RestaurantController : Controller
    {
        private readonly RestaurantsRepository _restaurantRepository;
        
        public RestaurantController(RestaurantsRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet("api/v1/restaurants/getrestaurant/")]
        [HttpGet("api/v1/restaurants/id/{restaurantId}")]
        public async Task<ActionResult> GetRestaurantAsync(string restaurantId, CancellationToken cancellationToken = default)
        {
            var matchedRestaurant = await _restaurantRepository.GetRestaurantAsync(restaurantId, cancellationToken);
            if (matchedRestaurant == null) return BadRequest(new ErrorResponse("Not found"));
            return Ok(new RestaurantResponse(matchedRestaurant));
        }

        [HttpGet("api/v1/restaurants/")]
        [HttpGet("api/v1/restaurants/search")]
        public async Task<ActionResult> GetRestaurantsAsync(int limit = 20, [FromQuery(Name = "page")] int page = 0,
            string sort = "name", int sortDirection = -1,
            CancellationToken cancellationToken = default)
        {
            var restaurants = await _restaurantRepository.GetRestaurantsAsync(limit, page, cancellationToken);

            var restaurantCount = page == 0 ? await _restaurantRepository.GetRestaurantsCountAsync() : -1;
            return Ok(new RestaurantResponse(restaurants, restaurantCount, page, null));
        }

        [HttpGet("api/v1/restaurants/")]
        public async Task<ActionResult> GetRestaurantsByTextAsync(CancellationToken cancellationToken = default, int page = 0,   params string[] keywords)
        {
            var restaurants = await _restaurantRepository.GetRestaurantsByTextAsync( cancellationToken, page, keywords);
            var restaurantCount = page == 0 ? await _restaurantRepository.GetRestaurantsCountAsync() : -1;
            return Ok(new RestaurantResponse(restaurants, restaurantCount, page, null));
        }

    }
}