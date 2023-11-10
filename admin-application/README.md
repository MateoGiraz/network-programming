# API Documentation

## Create Product
### Request
```http
POST /product
```
### Payload
```js
{
  "name": "string",
  "description": "string",
  "stock": 0,
  "price": 0,
  "credentials": {
    "username": "string",
    "password": "string"
  }
}
```
### Response
```js
{
  "result": "string"
}
```
## Update Product
### Request
```http
PUT /product
```
### Payload
```js
{
  "name": "string",
  "description": "string",
  "stock": 0,
  "price": 0,
  "credentials": {
    "username": "string",
    "password": "string"
  }
}
```
### Response
```js
{
  "result": "string"
}
```
## Delete Product
### Request
```http
DELETE /product/{name}
```
### Payload
```js
{
  "username": "string",
  "password": "string"
}
```
### Response
```js
{
  "result": "string"
}
```
## Buy Product
### Request
```http
POST /buy/{name}
```
### Payload
```js
{
  "username": "string",
  "password": "string"
}
```
### Response
```js
{
  "result": "string"
}
```
## Get Rating
### Request
```http
GET /rating/{name}
```
### Payload
```js
{
  "username": "string",
  "password": "string"
}
```
### Response
```js
{
  "result": "string"
}
```
