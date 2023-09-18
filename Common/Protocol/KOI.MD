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
