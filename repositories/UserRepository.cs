using System;
using System.Threading;
using System.Threading.Tasks;
using RestaurantReview.Models;
using RestaurantReview.Models.Responses;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Collections.Generic;

namespace RestaurantReview.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<Session> _sessionsCollection;

        public UserRepository(IMongoClient mongoClient)
        {
            var camelCaseConvention = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _usersCollection = mongoClient.GetDatabase("sample_restaurants").GetCollection<User>("users");
            _sessionsCollection = mongoClient.GetDatabase("sample_restaurants").GetCollection<Session>("sessions");
        }

        public async Task<User> GetUserAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync(cancellationToken);
            Console.WriteLine($"USER = {user.Name}");
            return user;
            
         
        }

        public async Task<UserResponse> CreateUserAsync(string name, string password, string email, CancellationToken cancellationToken = default)
        {
            try{
                

             var user = new User
             {
                Email = email,
                Password = PasswordManager.Hash(password),
                Name = name,
            };
          
                await _usersCollection.InsertOneAsync(user);

                var newUser = await GetUserAsync(email, cancellationToken);

                return new UserResponse(newUser);
            }
            catch(Exception ex)
            {   
                Console.WriteLine( $"NEWUSERd = {ex.Message}");  
                return new UserResponse(false, ex.Message);
            }
        }

        public async Task<UserResponse> LoginUserAsync(string email, string password, string authToken, CancellationToken cancellationToken = default)
        {
            try
            {   

                var user = new User{
                    Email = email,
                    Password =password,
                    AuthToken = authToken
                };
                
                var userFromDB = await GetUserAsync(email, cancellationToken);
                if (userFromDB == null)
                {
                    return new UserResponse(false,"User not found");
                }

  
              
                
                if(!PasswordManager.Verify(password,userFromDB.Password))
                {
                    return new UserResponse(false,"Invalid password hash");
                }


                await _sessionsCollection.UpdateOneAsync(new BsonDocument("user_id", user.Email),
                    Builders<Session>.Update.Set(s => s.UserId, user.Email)
                    .Set(s => s.Jwt, user.AuthToken),
                    new UpdateOptions { IsUpsert = true },
                    cancellationToken);

                userFromDB.AuthToken = user.AuthToken;
                
                return new UserResponse(userFromDB);
                
            }
            catch (Exception ex)
            {
                return new UserResponse(false, ex.Message);
            }
        }

        public async Task<UserResponse> LogoutUserAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
             var result = await _sessionsCollection.DeleteOneAsync(new BsonDocument("user_id", email), cancellationToken);
                return new UserResponse(true, "User logged out");
                
            }
            catch (Exception ex)
            {
                return new UserResponse(false, ex.Message);
            }
        }

        public async Task<Session> GetSessionAsync(string email, CancellationToken cancellationToken)
        {
            return await _sessionsCollection.Find(new BsonDocument("user_id", email)).FirstOrDefaultAsync(cancellationToken);
        }


    }

}