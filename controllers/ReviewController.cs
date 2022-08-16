using System;
using System.Linq;
using System.Threading.Tasks;
using RestaurantReview.Models.Responses;
using RestaurantReview.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace RestaurantReview.Controllers
{
    public class ReviewController : ControllerBase
    {
        private readonly ReviewRepository _reviewsRepository;
        private readonly IOptions<JwtAuthentication> _jwtAuthentication;
        private readonly UserRepository _userRepository;

        public ReviewController(ReviewRepository reviewsRepository,
            UserRepository userRepository, IOptions<JwtAuthentication> jwtAuthentication)
        {
            _reviewsRepository = reviewsRepository;
            _userRepository = userRepository;
            _jwtAuthentication = jwtAuthentication ?? throw new ArgumentNullException(nameof(jwtAuthentication));
        }

        [HttpPost("/api/v1/movies/review")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> AddReview([FromBody] RestaurantReviewInput input)
        {
            var user = await UserController.GetUserFromTokenAsync(_userRepository, Request);

            var restaurantId = new ObjectId(input.RestaurantId);
            var result = await _reviewsRepository.AddReviewAsync(user, restaurantId, input.Review);

            return result != null
                ? (ActionResult)Ok(new ReviewResponse(result.Reviews.OrderByDescending(d => d.Date).ToList()))
                : BadRequest(new ReviewResponse());
        }

        [HttpPut("/api/v1/movies/review")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> UpdateReviewAsync([FromBody] RestaurantReviewInput input)
        {
            var user = await UserController.GetUserFromTokenAsync(_userRepository, Request);

            var restaurantId = new ObjectId(input.RestaurantId);
            var reviewId = new ObjectId(input.reviewId);
            var result = await _reviewsRepository.UpdateReviewAsync( restaurantId, reviewId,  user, input.Review);

            return result != null
                ? (ActionResult)Ok(new ReviewResponse())
                : BadRequest(new ReviewResponse());
        }

        [HttpDelete("/api/v1/movies/review")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> DeleteReviewAsync([FromBody] RestaurantReviewInput input)
        {
            var restaurantId = new ObjectId(input.RestaurantId);
            var reviewId = new ObjectId(input.reviewId);
            var user = await UserController.GetUserFromTokenAsync(_userRepository, Request);
            var result = await _reviewsRepository.DeleteReviewAsync(restaurantId, reviewId, user);

            return result != null
                ? (ActionResult)Ok(new ReviewResponse())
                : BadRequest(new ReviewResponse());
        }
    }

    public class RestaurantReviewInput
    {
        [JsonProperty("restaurant_id")]
        public string RestaurantId { get; set; }

        [BsonId]
        [JsonProperty("review_id")]
        public string reviewId { get; set; }
        public string Review { get; set; }

        [JsonProperty("updated_review")]
        public string UpdatedReview { get; set; }
    }
}
