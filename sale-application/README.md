# API Documentation

## Get Sales
### Request
```http
GET /sale
```
## Query Parameters
```
username (string, optional): Filter sales by username
```
```
name (string, optional): Filter sales by product name
```
```
description (string, optional): Filter sales by product description
```
### Response
```js
[
  {
    "username": "string",
    "product": {
        "name": 0,
        "description": "string",
        "price": 0
    }
  }
]
```
