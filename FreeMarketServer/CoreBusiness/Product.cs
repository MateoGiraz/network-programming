
namespace CoreBusiness;
using System.Drawing;


public class Product
{
     public string Name { get; set; }
     public string Description { get; set; }
     public int Stock { get; set; }
     public int Price { get; set; }
     public List<Rating> Ratings { get; set; }
     public Owner Owner { get; set; }
     public string ImageRoute { get; set; }

     public void AddRating(int score, string comment)
     {
          Ratings ??= new List<Rating>();
          
          var toAddRating = new Rating()
          {
               Score = score,
               Comment = comment
          };
          
          Ratings.Add(toAddRating);
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