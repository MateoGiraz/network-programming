using CoreBusiness;
using BusinessLogic;

/* *** DISCLAIMER ***
 Solo existe la relacion con CoreBusiness para testear. Cuando esto funcione no necesitamos
 el objeto producto, sino que vamos a representarlo como json o algo del estilo xd.
 */

ProductController pr = new();
/*Cuando tengamos la parte de cliente ya no necesitariamos tener al cliente como variable*/
Owner mockClient = new Owner
{
    userName = "Joaquin",
    password = "A12345"
};

Owner mockClient2 = new Owner
{
    userName = "Mateo",
    password = "A12345"
};


var patata = new Product()
{
    Name = "Patata",
    Description = "Es una patata bro",
    Price = 10,
    Stock = 5,
    Client = mockClient
};

var tomato = new Product()
{
    Name = "Tomato",
    Description = "No se que esperabas",
    Price = 120,
    Stock = 12,
    Client = mockClient2
};

pr.AddProduct(patata);
pr.AddProduct(tomato);

pr.BuyProduct(patata, 3);

var list = pr.GetProducts();

foreach (var product in list)
{
    Console.WriteLine("There are " + product.Stock + " " + product.Name + " Owner: " + product.Client.userName);
}
