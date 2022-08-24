using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestaurantReview.Models;
using RestaurantReview.Models.Projections;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace RestaurantReview.Repositories
{
    public class RestaurantsRepository
    {
        private const int RestaurantsPerPage = 12;
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

        public async Task<IReadOnlyList<Restaurant>> GetRestaurantsAsync(int perPage = RestaurantsPerPage, int page = 0,
        
            CancellationToken cancellationToken = default)
        {
           var skip = page * perPage;
           var limit = perPage;
            var restaurants = await _restaurantsCollection.Find(Builders<Restaurant>.Filter.Empty)  
            
                .Skip(skip)
                .Limit(limit)
                .ToListAsync(cancellationToken);
            return restaurants;
        }

        public async Task<Restaurant> GetRestaurantAsync(string restaurantId, CancellationToken cancellationToken = default)
        {
           /* var restaurant = await _restaurantsCollection.Find(Builders<Restaurant>.Filter.Eq(r => r.Id, restaurantId))
                .FirstOrDefaultAsync(cancellationToken);
            return restaurant;*/
            Console.WriteLine("RestaurantId: " + restaurantId);
            var test = await _restaurantsCollection.Aggregate()
                .Match(Builders<Restaurant>.Filter.Eq(x => x.Id, restaurantId))
                .Lookup  (
                _reviewsCollection, 
                r => r.Id, 
                c => c.RestaurantId, 
                (Restaurant r) => r.Reviews)
                .FirstOrDefaultAsync(cancellationToken);

            Console.WriteLine("test: " + test.Reviews);
            var restuarantObject = new ObjectId(restaurantId);
            
            var y = await _reviewsCollection.Aggregate().Match(Builders<Review>.Filter.Eq(x => x.RestaurantId, restuarantObject)).ToListAsync(cancellationToken);
            Console.WriteLine("test: " + y.Count);
            
          

            return await  _restaurantsCollection.Aggregate()
                .Match(Builders<Restaurant>.Filter.Eq(x => x.Id, restaurantId))
                .Lookup  (
                _reviewsCollection, 
                r => r.Id, 
                c => c.RestaurantId, 
                (Restaurant r) => r.Reviews)
                .FirstOrDefaultAsync(cancellationToken);
        }
       
      
        
        public async Task<long> GetRestaurantsCountAsync(){
            return await _restaurantsCollection.CountDocumentsAsync(Builders<Restaurant>.Filter.Empty);
        }

        public async Task<IReadOnlyList<Restaurant>> GetRestaurantsByTextAsync(CancellationToken cancellationToken = default,  int page = 0,  params string[] keywords)
        {
             return await _restaurantsCollection
                .Find(Builders<Restaurant>.Filter.Text(string.Join(",", keywords)))
                
                .Limit(RestaurantsPerPage)
                .Skip(page * RestaurantsPerPage)
                .ToListAsync(cancellationToken);
            
        }
    }
}