[Back to README](./README.md)

## General API Definitions

**Note**

The tests will use this sequence of api calls:

Creation of several products:

POST: http://ocelot-gateway:7777/products
```json

{
    "price" : 9999,
    "description" : "XXXXX",
    "category" : "XXXXX",
    "image" : "XXXXX"
};
```
GET: http://ocelot-gateway:7777/products
```json

{"data":
    [
    {"id":"01953a21-e4ac-7cff-bb9b-d1277f63c9b2",
    "title":"titulo 1",
    "price":5.00,
    "description":"Descricao do titulo q",
    "category":"Categoria 1",
    "image":"imagem"},
    {"id":"01953a37-bdc9-7acd-b9f5-b638de629994",
    "title":"titulo 3",
    "price":5.00,
    "description":"Descricao do titulo 3",
    "category":"Categoria 1",
    "image":"imagem"}
....]}
```
Sales Process

POST: http://ocelot-gateway:7777/sales

```json

Input:
{
	"SaleNumber" : "5977", //*** aleatory number
	"SaleDate" : "2025-02-25T20:18:18Z",
	"CustomerI": "bdd6dbfc-ff0a-4b0f-8509-e4d91f0b0dde", // **** any Guid.NewGuid()
	"BranchId": "3d4bc7d5-fcd2-4b07-a807-b8212afbd17d", 	//**** any Guid.NewGuid()
	"Items" : [
				{ 	"ProductId": "01953a37-bdc9-7acd-b9f5-b638de629994",
					"Quantity": 3,
					"UnitPrice": 5.00,
				},
				{	"ProductId": "01953a21-e4ac-7cff-bb9b-d1277f63c9b2",
					"Quantity": 2,
					"UnitPrice": 5.00,
				}
			]
}
```

```json

output:
{
	"data": {
		"id": "6772b1bd-da23-4526-b2e0-e92f1752b805",
		"saleNumber": "5977",
		"date": "2025-02-25T20:22:13.2142894Z",
		"customerId": "bdd6dbfc-ff0a-4b0f-8509-e4d91f0b0dde",
		"branchId": "3d4bc7d5-fcd2-4b07-a807-b8212afbd17d",
		"totalAmount": 25.0,
		"cancelled": false,
		"items": [
			{
				"id": "8620c2a3-3033-4e94-a0ec-3be12917c24e",
				"productId": "01953a37-bdc9-7acd-b9f5-b638de629994",
				"quantity": 3,
				"unitPrice": 5.0,
				"valueMonetaryTaxApplied": 0.0,
				"total": 15.0,
				"saleId": "6772b1bd-da23-4526-b2e0-e92f1752b805",
				"isCancelled": false
			},
			{
				"id": "1d8a48b8-f232-4abe-ad66-98f364a47194",
				"productId": "01953a21-e4ac-7cff-bb9b-d1277f63c9b2",
				"quantity": 5,
				"unitPrice": 5.0,
				"valueMonetaryTaxApplied": 2.5,
				"total": 22.5,
				"saleId": "6772b1bd-da23-4526-b2e0-e92f1752b805",
				"isCancelled": false
			}
		]
	},
	"status": "success",
	"message": "Venda criada com sucesso"
}
```

List sells:

GET: http://ocelot-gateway:7777/sales

```json
{
  "data": [
    {
      "id": "01657bfb-94b4-4d80-aaea-d2552d77691e",
      "saleNumber": "8909",
      "date": "2025-02-25T19:35:30.588332Z",
      "customerId": "a3bbb371-fa86-4fb9-b803-2c1719920b1f",
      "branchId": "13f72635-f956-439c-8bf2-77443e7907b8",
      "totalAmount": 25.0,
      "cancelled": true,
      "items": [
        {
          "id": "52947910-d27e-494e-af21-44c95641edac",
          "productId": "01953a21-e4ac-7cff-bb9b-d1277f63c9b2",
          "quantity": 2,
          "unitPrice": 5.0,
          "valueMonetaryTaxApplied": 0.0,
          "total": 10.0,
          "saleId": "01657bfb-94b4-4d80-aaea-d2552d77691e",
          "isCancelled": false
        },
        {
          "id": "d5735ff2-55a4-469c-9471-d5160538621a",
          "productId": "01953a37-bdc9-7acd-b9f5-b638de629994",
          "quantity": 3,
          "unitPrice": 5.0,
          "valueMonetaryTaxApplied": 0.0,
          "total": 15.0,
          "saleId": "01657bfb-94b4-4d80-aaea-d2552d77691e",
          "isCancelled": false
        }
      ]
    },
    {
      "id": "0822832c-0460-48c9-abdc-4b986a46b0f5",
      "saleNumber": "6343",
      "date": "2025-02-25T19:49:48.546794Z",
      "customerId": "6cf3580a-117b-402d-80ff-0925c65ed5ba",
      "branchId": "56849e89-f4b2-42fc-a247-ed4fb7760c6b",
      "totalAmount": 84.5,
      "cancelled": false,
      "items": [
        {
          "id": "01b025eb-d478-4383-bcf0-e4f83653f414",
          "productId": "0195..."
        }
      ]
    }
  ]
}
```

if sell above 20 identical items:

```json
{
  "type": "BadRequest",
  "error": "Invalid Sell",
  "detail": "You cannot buy more than 20 pices of same item"
}
```

Cancelling one sales:

DELETE: http://ocelot-gateway:7777/sales/{Id}

output:
{
"status": "success",
"message": "Sell Cancelled"
}

## Error Handling

The API uses conventional HTTP response codes to indicate the success or failure of an API request. In general:

- 2xx range indicate success
- 4xx range indicate an error that failed given the information provided (e.g., a required parameter was omitted, etc.)
- 5xx range indicate an error with our servers

### Error Response Format

```json
{
  "type": "string",
  "error": "string",
  "detail": "string"
}
```

- `type`: A machine-readable error type identifier
- `error`: A short, human-readable summary of the problem
- `detail`: A human-readable explanation specific to this occurrence of the problem

Example error responses:

1. Resource Not Found

```json
{
  "type": "InvalidData",
  "error": "Price cannot be empty",
  "detail": "The sales with ID 12345 has the product XXXX with zero price"
}
```

For detailed error information, refer to the specific endpoint documentation.
