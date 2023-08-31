
namespace CoreBusiness;
using System.Drawing;

public class Product
{
     public string Name;
     public string Description;
     public int Stock;
     public int Price;
     public List<Rating> ratings;

     public void AddRating(int score, string comment)
     {
          var toAddRating = new Rating()
          {
               Score = score,
               Comment = comment
          };
          
          ratings.Add(toAddRating);
     }
}