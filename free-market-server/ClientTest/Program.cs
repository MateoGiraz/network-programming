using CoreBusiness;
using BusinessLogic;
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


List<Owner> owners=or.GetOwners();
foreach (var owner in owners)
{
    Console.WriteLine(" Owner: "+ owner.UserName);
}

or.LogIn(user.UserName,user.Password);
or.LogIn(user2.UserName,user2.Password);
or.LogIn(user3.UserName,user3.Password);



var patata = new Product()
{
    Name = "Patata",
    Description = "Es una patata bro",
    Price = 10,
    Stock = 5,
    Owner = user
};

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


