using System;
using System.Threading;
using System.Threading.Tasks;
using Restaurant.Models;
using Restaurant.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Restaurant.Controllers
{


    public class RestaurantControllers : Controller
    {
        private readonly RestaurantRepository _restaurantRepository;
        public RestaurantController(RestaurantRepository restaurantRepository)
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
            var restaurants = await _restaurantRepository.GetRestaurantsAsync(limit, page, sort, sortDirection, cancellationToken);

            var restaurantCount = page == 0 ? await _restaurantRepository.GetRestaurantsCountAsync() : -1;
            return Ok(new RestaurantResponse(restaurants, restaurantCount, page, null));
        }

        [HttpGet("api/v1/restaurants/")]
        public async Task<ActionResult> GetRestaurantsByTextAsync(page: page, keywords: keywords, limit: limit, sort: sort, sortDirection: sortDirection, cancellationToken = default)
        {
            var restaurants = await _restaurantRepository.GetRestaurantsByTextAsync(page, keywords, limit, sort, sortDirection, cancellationToken);
            var restaurantCount = page == 0 ? await _restaurantRepository.GetRestaurantsCountAsync() : -1;
            return Ok(new RestaurantResponse(restaurants, restaurantCount, page, null));
        }

    }
}