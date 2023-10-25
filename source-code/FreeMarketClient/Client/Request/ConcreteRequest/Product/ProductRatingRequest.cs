using Common.DTO;

namespace free_market_client.Request.ConcreteRequest.Product;

public class ProductRatingRequest : ProductRequest
{
    protected override async Task HandleConcreteProductOperationAsync()
    {

        var score = InputHelper.GetValidRatingInput("Add Rating Score");
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
    protected override async Task HandleImageSendingAsync() {}
}