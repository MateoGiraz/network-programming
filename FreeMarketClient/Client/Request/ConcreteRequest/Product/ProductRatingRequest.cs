using Common.DTO;

namespace free_market_client.Request.ConcreteRequest.Product;

public class ProductRatingRequest : ProductRequest
{
    protected override void HandleConcreteProductOperation()
    {

        var score = InputHelper.GetInputWithoutHash("Add Rating Score");
        var comment = InputHelper.GetInputWithoutHash("Add Rating Comment");
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
    protected override void HandleImageSending() {}
}