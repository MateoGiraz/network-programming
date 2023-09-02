using CoreBusiness;
using BusinessLogic;

/* *** DISCLAIMER ***
 Solo existe la relacion con CoreBusiness para testear. Cuando esto funcione no necesitamos
 el objeto producto, sino que vamos a representarlo como json o algo del estilo xd.
 */

ProductController pr = new();

var patata = new Product()
{
    Name = "Patata",
    Description = "Es una patata bro",
    Price = 10,
    Stock = 5,
};

pr.AddProduct(patata);

pr.BuyProduct(patata, 3);

var list = pr.GetProducts();

foreach (var product in list)
{
    Console.WriteLine("There are " + product.Stock + " " + product.Name);
}
