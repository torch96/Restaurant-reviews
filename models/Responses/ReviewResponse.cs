using System.Collections.Generic;
using RestaurantReview.Models.Projections;

namespace RestaurantReview.Models.Responses
{
    public class ReviewResponse
    {
        public ReviewResponse()
        {
            Reviews = new List<Review>();
         
        }
        public ReviewResponse(List<Review> reviews)
        {
            Reviews = reviews;
        }
        
        public IReadOnlyList<Review> Reviews { get; set; }
    }
}