using Common.Protocol;
using BusinessLogic;

namespace ServerConnection.Handler.Product.ConcreteProductHandler;

public class ProductRatingHandler : ProductHandler
{
    protected override async Task HandleProductSpecificOperationAsync()
    {
        var ratingList = KOI.GetObjectMapList(ProductMap!["Ratings"]);
        var newRating = ratingList[0];
        
        var pc = new ProductController();
        var toAddRatingProduct = pc.GetProduct(ProductDto!.Name);

        var score = int.Parse(newRating["Score"]);
        var comment = newRating["Comment"];
        
        toAddRatingProduct.AddRating(score, comment);
        
        ResponseDto!.StatusCode = 200;
        ResponseDto.Message = $"Added Rating score: {score}, comment: {comment} to Product {toAddRatingProduct.Name}";
    }
}