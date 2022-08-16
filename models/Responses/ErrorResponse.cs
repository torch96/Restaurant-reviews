namespace RestaurantReview.Models.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(string errorMessage)
        {
            Error = errorMessage;
        }
        public string Error { get; set; }
    }
}