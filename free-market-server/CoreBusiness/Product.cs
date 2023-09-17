
namespace CoreBusiness;
using System.Drawing;


public class Product
{
     public string Name { get; set; }
     public string Description { get; set; }
     public int Stock { get; set; }
     public int Price { get; set; }
     public List<Rating> ratings { get; set; }
     public Owner Owner { get; set; }
     public byte[] ImageData { get; set; }

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
     public void SetImage(string imagePath)
     {
          ImageData = File.ReadAllBytes(imagePath);
     }

// Method to get image
     public Image GetImage()
     {
          using (var ms = new MemoryStream(ImageData))
          {
               return Image.FromStream(ms);
          }
     }
}