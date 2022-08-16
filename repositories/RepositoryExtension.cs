using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RestaurantReview.Repositories
{
    public static class RepositoryExtensions
    {
        public static void RegisterMongoDbRepositories(this IServiceCollection servicesBuilder)
        {
            servicesBuilder.AddSingleton<IMongoClient, MongoClient>(s =>
            {
                var uri = s.GetRequiredService<IConfiguration>()["MongoUri"];
                return new MongoClient(uri);
            });
            servicesBuilder.AddSingleton<RestaurantsRepository>();
            servicesBuilder.AddSingleton<ReviewRepository>();
            servicesBuilder.AddSingleton<UserRepository>();
           
            servicesBuilder.AddSingleton(s => s.GetRequiredService<IConfiguration>()["JWT_SECRET"]);
        }
    }
}                                   