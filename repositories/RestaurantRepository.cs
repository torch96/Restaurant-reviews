using System;
using System.collection.Generic;
using System.Threading;
using System.Threading.Tasks;
using Restaurant.Models;
using Restaurant.Models.Projections;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Restaurant.Repositories
{
    public class RestaurantsRepository
    {
        private const int RestaurantsPerPage = 10;
        private readonly IMongoCollection<Restaurant> _restaurantsCollection;
        private readonly IMongoCollection<Review> _reviewsCollection;
        private readonly IMongoClient _mongoClient;

        public RestaurantsRepository(IMongoClient mongoClient)
        {
            var camelCaseConvention = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _restaurantsCollection = mongoClient.GetDatabase("sample_restaurants").GetCollection<Restaurant>("restaurants");
            _reviewsCollection = mongoClient.GetDatabase("sample_restaurants").GetCollection<Review>("reviews");
            _mongoClient = mongoClient;
        }

        public async Task<IReadOnlyList<Restaurant>> GetRestaurantsAsync(int page, CancellationToken cancellationToken = default)
        {
            var restaurants = await _restaurantsCollection.Find(new BsonDocument())
                .Sort(Builders<Restaurant>.Sort.Ascending(r => r.Name))
                .Skip(RestaurantsPerPage * (page - 1))
                .Limit(RestaurantsPerPage)
                .ToListAsync(cancellationToken);
            return restaurants;
        }
    }
}