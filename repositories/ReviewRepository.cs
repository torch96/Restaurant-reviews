using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using RestaurantReview.Models;
using RestaurantReview.Models.Projections;
using RestaurantReview.Models.Responses;


namespace RestaurantReview.Repositories
{
    public class ReviewRepository
    {
        private readonly IMongoCollection<Review> _reviewsCollection;
        private readonly  RestaurantsRepository _restaurantsRepository;

        public ReviewRepository(IMongoClient mongoClient)
        {
            var camelCaseConvention = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _reviewsCollection = mongoClient.GetDatabase("sample_restuarants").GetCollection<Review>("reviews");
            _restaurantsRepository = new RestaurantsRepository(mongoClient);
        }

        public async Task<Restaurant> AddReviewAsync(User user, ObjectId restaurantId, string review,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var newReview = new Review
                {
                    Date = DateTime.UtcNow,
                    Text = review,
                    Name = user.Name,
                    Email = user.Email,
                    RestaurantId = restaurantId
                };
                await _reviewsCollection.InsertOneAsync(newReview);
                return await _restaurantsRepository.GetRestaurantAsync(restaurantId.ToString(), cancellationToken);
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        public async Task<UpdateResult> UpdateReviewAsync(ObjectId restaurantId, ObjectId reviewId, User user, string review,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var filter = Builders<Review>.Filter.Eq(r => r.Id, reviewId);
                var update = Builders<Review>.Update
                    .Set(r => r.Text, review);
                return await _reviewsCollection.UpdateOneAsync(filter, update);
            }
            catch
            {
                return null;
            }
        }

        public async Task<DeleteResult> DeleteReviewAsync(ObjectId restaurantId, ObjectId reviewId, User user, CancellationToken cancellationToken = default)
        {
            try
            {
                var filter = Builders<Review>.Filter.Eq(r => r.Id, reviewId);
                return await _reviewsCollection.DeleteOneAsync(filter, cancellationToken);
            }
            catch
            {
                return null;
            }
        }


    }
}