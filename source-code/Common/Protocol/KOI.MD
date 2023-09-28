## KOI docs
### Usage
Use Stringify method to convert an object to string.

```js
Owner user = new Owner
{
    UserName = "Joaquin",
    Password = "A12345"
};
var encodedUser = KOI.Stringify(user);
```
Use Parse method to get a map from a stringified object.
```js
var userDic = KOI.Parse(encodedUser);
```
Access simple object attributes by using its original properties as map keys.
```js
var userName = userDic["UserName"];
var password = userDic["Password"];
```
Access object attributes by using their original name as key, and get its map.
```js
Product Potato = new Product()
{
    Name = "Potato",
    Description = "Just Potato",
    Price = 10,
    Stock = 5,
    Owner = user,
};

var encodedProduct = KOI.Stringify(Potato);
var productDic = KOI.Parse(encodedProduct);

var productOwner = productDic["Owner"];
var ownerMap = KOI.GetObjectMap(productOwner);

var ownerUsername = ownerMap["UserName"];
var ownerPassword = ownerMap["Password"];
```
If Parsed object held a list of objects, use its original name to get a list of Maps.

```js
var TomatoRatings = new List<Rating>()
{
    new()
    {
        Score = 1, Comment = "Meh"
    },
    new()
    {
        Score = 10, Comment = "Eggscellent!"
    }
};

Product Tomato = new Product()
{
    Name = "Tomato",
    Description = "Fresh Tomato",
    Price = 5,
    Stock = 1,
    Owner = user,
    Ratings = TomatoRatings,
};

var encodedProduct = KOI.Stringify(Tomato);
var productDic = KOI.Parse(encodedProduct);

var tomatoRating = productDic["Ratings"];
var tomatoRatingList = KOI.GetObjectMapList(tomatoRating);

for(var rating in tomatoRatingList) {
    Console.WriteLine("Rating Score: " + rating["Score"]);
    Console.WriteLine("Rating Comment: " + rating["Comment"]);
}
```

## KOI Product Sending Example
### Warning! KOI must receive DTO which complex fields are defined after primitive fields.

Define DTO as follows

```csharp
public class ProductDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Stock { get; set; }
    public string Price { get; set; }
    public string ImageData { get; set; }
    public List<RatingDTO> Ratings { get; set; }
    public UserDTO Owner { get; set; }
}
```

DO NOT define any primitive prop after a complex prop (object, object list)
```csharp
public class ProductDTO
{
    public List<RatingDTO> Ratings { get; set; }
    public UserDTO Owner { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Stock { get; set; }
    public string Price { get; set; }
    public string ImageData { get; set; }
}
```

## Creating Product DTO
```js
Console.WriteLine("Type Username");
var user = Console.ReadLine();

Console.WriteLine("Type Password");
var password = Console.ReadLine();

var userDTO = new UserDTO()
{
    UserName = user,
    Password = password
};

Console.WriteLine("Enter Comment1");
var comment1 = Console.ReadLine();

Console.WriteLine("Enter Rating1");
var rating1 = Console.ReadLine();

Console.WriteLine("Enter Comment2");
var comment2 = Console.ReadLine();

Console.WriteLine("Enter Rating2");
var rating2 = Console.ReadLine();

var rat1 = new RatingDTO()
{
    Comment = comment1,
    Score = rating1
};

var rat2 = new RatingDTO()
{
    Comment = comment2,
    Score = rating2
};

var ratings = new List<RatingDTO>()
{
    rat1,
    rat2
};

Console.WriteLine("Type Product name");
var name = Console.ReadLine();

Console.WriteLine("Type Price");
var price = Console.ReadLine();

Console.WriteLine("Type Product Stock");
var stock = Console.ReadLine();

Console.WriteLine("Type Description");
var description = Console.ReadLine();

Console.WriteLine("Type Image data");
var img = Console.ReadLine();

var productDTO = new ProductDTO()
{
    Price = price,
    Name = name,
    Description = description,
    ImageData = img,
    Stock = stock,
    Owner = userDTO, 
    Ratings = ratings
};

var userData = KOI.Stringify(productDTO);
```

## Receiving Product DTO
```js
var prodMap = KOI.Parse(productString);

Console.WriteLine("Name: "+prodMap["Name"]);
Console.WriteLine("Description: "+prodMap["Description"]);
Console.WriteLine("Stock: "+prodMap["Stock"]);
Console.WriteLine("Price: "+prodMap["Price"]);
Console.WriteLine("ImgData: "+prodMap["ImageData"]);

var owner = KOI.GetObjectMap(prodMap["UserDto"]);
Console.WriteLine("User: "+owner["UserName"]);
Console.WriteLine("Password: "+owner["Password"]);

var ratings = KOI.GetObjectMapList(prodMap["bananas"]);

foreach (var rating in ratings)
{
    Console.WriteLine("Comment: "+rating["Comment"]);
    Console.WriteLine("Score: "+rating["Score"]);
}
        
```

