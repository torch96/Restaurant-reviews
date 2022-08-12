using System;
using System.Threading;
using System.Threading.Tasks;
using Restaurant.Models;
using Restaurant.Models.Projections;
using Restaurant.Models.Responses;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Restuarant.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<Session> _sessionsCollection;

        public UserRepository(IMongoClient mongoClient)
        {
            var camelCaseConvention = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _usersCollection = mongoClient.GetDatabase("sample_restuarants").GetCollection<User>("users");
            _sessionsCollection = mongoClient.GetDatabase("sample_restaurants").GetCollection<Session>("sessions");
        }

        public async Task<User> GetUserAsync(string email, CancellationToken cancellationToken)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await _usersCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<UserResponse> CreateUserAsync(string name, string password, string email, CancellationToken cancellationToken)
        {
            try{
                var user = new User();

             user = new User
             {
                Email = Email,
                Password = Password,
                Name = name,
            };
                await _usersCollection.InsertOneAsync(user);

                var newUser = await GetUserAsync(user.Email, cancellationToken);
                return new UserResponse(newUser);
            }
            catch(Exception ex)
            {
                return new UserResponse
                {
                    Error = ex.Message
                };
            }
        }

        public async Task<UserResponse> LoginUserAsync(string email, string password, CancellationToken cancellationToken)
        {
            try
            {
                var user = await GetUserAsync(email, cancellationToken);
                if (user == null)
                {
                    return new UserResponse
                    {
                        Error = "User not found"
                    };
                }
                if (user.Password != password)
                {
                    return new UserResponse
                    {
                        Error = "Password is incorrect"
                    };
                }

                await _sessionsCollection.UpdateOneAsync(new BsonDocument("user_id", user.Email),
                    Builders<Session>.Update.Set(s => s.UserId, user.Email)
                    .Set(s => s.Jwt, user.AuthToken),
                    new UpdateOptions { IsUpsert = true },
                    cancellationToken);
            }
            catch (Exception ex)
            {
                return new UserResponse
                {
                    Error = ex.Message
                };
            }
        }

        public async Task<UserResponse> LogoutUserAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var user = await GetUserAsync(email, cancellationToken);
                if (user == null)
                {
                    return new UserResponse
                    {
                        Error = "User not found"
                    };
                }
                var filter = Builders<Session>.Filter.Eq(s => s.UserId, user.Id);
                var result = await _sessionsCollection.DeleteOneAsync(filter);
                if (result.DeletedCount == 0)
                {
                    return new UserResponse
                    {
                        Error = "User not found"
                    };
                }
                return new UserResponse
                {
                    User = user
                };
            }
            catch (Exception ex)
            {
                return new UserResponse
                {
                    Error = ex.Message
                };
            }
        }

        public async Task<Session> GetSessionAsync(string token, CancellationToken cancellationToken)
        {
            var filter = Builders<Session>.Filter.Eq(s => s.Token, token);
            return await _sessionsCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }


    }

}