
namespace CoreBusiness;
using System.Drawing;

public class Product
{
     public string Name;
     public string Description;
     public int Stock;
     public int Price;
     public List<Rating> ratings;
     public Owner Client;

     public void AddRating(int score, string comment)
     {
          var toAddRating = new Rating()
          {
               Score = score,
               Comment = comment
          };
          
          ratings.Add(toAddRating);
     }
     
     public override bool Equals(object obj)
     {
          //Check for null and compare run-time types.
          if ((obj is null) || ! this.GetType().Equals(obj.GetType()))
          {
               return false;
          }
          else {
               Product p = (Product) obj;
               return (Name == p.Name);
          }
     }
}