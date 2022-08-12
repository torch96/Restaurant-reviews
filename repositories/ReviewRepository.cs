using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Restaurant.Models;
using Restaurant.Models.Projections;
using Restaurant.Models.Responses;


namespace Restaurant.Repositories
{
    public class ReviewRepository
    {
        private readonly IMongoCollection<Review> _reviewsCollection;
        private readonly IMongoCollection<User> _restaurantsRepository;

        public ReviewRepository(IMongoClient mongoClient)
        {
            var camelCaseConvention = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _reviewsCollection = mongoClient.GetDatabase("sample_restuarants").GetCollection<Review>("reviews");
            _restaurantsRepository = mongoClient.GetDatabase("sample_restaurants").GetCollection<User>("users");
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
                return await _restaurantsRepository.Find(Builders<User>.Filter.Eq(u => u.Id, restaurantId)).FirstOrDefaultAsync(cancellationToken);
            }
            catch
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
                return await _reviewsCollection.UpdateOneAsync(filter, update, cancellationToken);
            }
            catch
            {
                return null;
            }
        }

        public async Task<DeleteResult> DeleteReviewAsync(ObjectId restaurantId, ObjectId reviewId, CancellationToken cancellationToken = default)
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