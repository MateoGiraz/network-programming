using CoreBusiness;
using BusinessLogic;
using ServerConnection;

/* *** DISCLAIMER ***
 Solo existe la relacion con CoreBusiness para testear. Cuando esto funcione no necesitamos
 el objeto producto, sino que vamos a representarlo como json o algo del estilo xd.
 */

ProductController pr = new();
OwnerController or = new();
/*Cuando tengamos la parte de cliente ya no necesitariamos tener al cliente como variable*/
Owner mockClient = new Owner
{
    UserName = "Joaquin",
    Password = "A12345"
};

Owner mockClient2 = new Owner
{
    UserName = "Mateo",
    Password = "A12345"
};

Owner mockClient3 = new Owner
{
    UserName = "Paxo",
    Password = "PA4CHO0"
};

or.LogIn(mockClient.UserName,mockClient.Password);
or.LogIn(mockClient2.UserName,mockClient2.Password);
or.LogIn(mockClient3.UserName,mockClient3.Password);



var patata = new Product()
{
    Name = "Patata",
    Description = "Es una patata bro",
    Price = 10,
    Stock = 5,
    Owner = mockClient
};

var tomato = new Product()
{
    Name = "Tomato",
    Description = "No se que esperabas",
    Price = 120,
    Stock = 12,
    Owner = mockClient2
};

var villa = new Product()
{
    Name = "Villa",
    Description = "Muy caro, mejor hacer una plaia",
    Price = 4000,
    Stock = 2,
    Owner = mockClient
};

var resort = new Product()
{
    Name = "Resort",
    Description = "Es un risolt papi",
    Price = 5000,
    Stock = 2,
    Owner = mockClient2
};

pr.AddProduct(patata);
pr.AddProduct(tomato);
pr.AddProduct(villa);
pr.AddProduct(resort);
pr.BuyProduct(patata, 3);

var list = pr.GetProducts();

foreach (var product in list)
{
    Console.WriteLine("There are " + product.Stock + " " + product.Name + " Owner: " + product.Owner.UserName);
}
Console.WriteLine("");
Console.WriteLine("PRUEBAS OWNER");
Console.WriteLine("PRUEBAS LOGIN");
or.LogIn("Joaco", "ReyDelTeteo");
or.LogIn("Paxo", "PA4CHO0");

List<Owner> owners=or.GetOwners();
foreach (var owner in owners)
{
    Console.WriteLine(" Owner: "+ owner.UserName);
}

pr.UpdateProduct(villa.Name,mockClient3,resort);
pr.UpdateProduct(villa.Name,mockClient2,resort);




var app = new Server();
app.Listen(3000);

