using Common.DTO;

namespace free_market_client.Request.ConcreteRequest.Product;

public class ProductRatingRequest : ProductRequest
{
    protected override void HandleConcreteProductOperation()
    {
        Console.WriteLine("Add Rating Score");
        var score = GetInputData();
        
        Console.WriteLine("Add Rating Comment");
        var comment = GetInputData();

        var ratingDto = new RatingDTO()
        {
            Comment = comment,
            Score = score,
        };
        
        var ratings = new List<RatingDTO>()
        {
            ratingDto
        };

        ProductDto!.Ratings = ratings;
    }
}