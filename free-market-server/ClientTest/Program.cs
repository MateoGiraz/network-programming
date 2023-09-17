using System.ComponentModel.Design;
using CoreBusiness;
using BusinessLogic;
using Common;
using ServerConnection;


/* *** DISCLAIMER ***
 Solo existe la relacion con CoreBusiness para testear. Cuando esto funcione no necesitamos
 el objeto producto, sino que vamos a representarlo como json o algo del estilo xd.
 */

ProductController pr = new();
OwnerController or = new();

/*Cuando tengamos la parte de cliente ya no necesitariamos tener al cliente como variable*/
Owner user = new Owner
{
    UserName = "Joaquin",
    Password = "A12345"
};
Console.WriteLine("test stringify user: ");

var encodedUser = KOI.Stringify(user);
Console.WriteLine(encodedUser);
var userDic = KOI.Parse(encodedUser);

Console.WriteLine(userDic["UserName"]);
Console.WriteLine(userDic["Password"]);


Owner user2 = new Owner
{
    UserName = "Mateo",
    Password = "A12345"
};

Owner user3 = new Owner
{
    UserName = "Paxo",
    Password = "PA4CHO0"
};

/*

List<Owner> owners=or.GetOwners();
foreach (var owner in owners)
{
    Console.WriteLine(" Owner: "+ owner.UserName);
}
*/
or.LogIn(user.UserName,user.Password);
or.LogIn(user2.UserName,user2.Password);
or.LogIn(user3.UserName,user3.Password);



Product patata = new Product()
{
    Name = "Patata",
    Description = "Es una patata bro",
    Price = 10,
    Stock = 5,
    Owner = user,
};

Console.WriteLine("test stringify product: ");
var encodedProduct = KOI.Stringify(patata);
var productDic = KOI.Parse(encodedProduct);

Console.WriteLine("Name: " + productDic["Name"]);
Console.WriteLine("Description: " + productDic["Description"]);
Console.WriteLine("Price: " + productDic["Price"]);
Console.WriteLine("Stock: " + productDic["Stock"]);

var productOwner = KOI.GetObjectMap(productDic["Owner"]);

Console.WriteLine("Owner UserName: " + productOwner["UserName"]);
Console.WriteLine("Owner Password: " + productOwner["Password"]);

/*
var tomato = new Product()
{
    Name = "Tomato",
    Description = "No se que esperabas",
    Price = 120,
    Stock = 12,
    Owner = user2
};

var villa = new Product()
{
    Name = "Villa",
    Description = "Muy caro, mejor hacer una plaia",
    Price = 4000,
    Stock = 2,
    Owner = user2
};

var resort = new Product()
{
    Name = "Resort",
    Description = "Es un risolt papi",
    Price = 5000,
    Stock = 2,
    Owner = user2
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

Console.WriteLine("");
Console.WriteLine("Login Exitoso:");
or.LogIn("Joaco", "ReyDelTeteo");
Console.WriteLine("Login Contraseña Incorrecta:");
or.LogIn("Paxo", "A12345");


pr.UpdateProduct(villa.Name,user3,resort);
pr.UpdateProduct(villa.Name,user2,resort);




var app = new Server();
app.Listen(3000);

*/