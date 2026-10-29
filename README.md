# CSE 325 - Week 01 Assignment

## .NET Applications with C#

This repository contains my completed Week 01 Microsoft Learn exercises using C# and .NET 8.

## Completed Modules

1. Write your first C# code
2. Introduction to .NET
3. Create a new .NET project and work with dependencies
4. Interactively debug .NET apps with Visual Studio Code
5. Work with files and directories in a .NET app
6. Create a web API with ASP.NET Core controllers

## Pizza API

The Pizza API supports GET, POST, PUT, and DELETE operations.

### Initial Pizza Inventory

| ID  | Name            | Gluten-Free |
| --- | --------------- | ----------- |
| 1   | Classic Italian | No          |
| 2   | Veggie          | Yes         |
| 3   | Mexican Fiesta  | No          |

Mexican Fiesta is the additional pizza I added for the assignment.

## CRUD Test Results

I tested the API locally at `http://localhost:5149`. The following are the requests, HTTP status codes, and responses from my tests.

### GET - Retrieve All Pizzas

**Request:**

```http
GET http://localhost:5149/pizza/
Accept: application/json
```

**HTTP Status:** 200 OK

**Response:**

```json
[
	{
		"id": 1,
		"name": "Classic Italian",
		"isGlutenFree": false
	},
	{
		"id": 2,
		"name": "Veggie",
		"isGlutenFree": true
	},
	{
		"id": 3,
		"name": "Mexican Fiesta",
		"isGlutenFree": false
	}
]
```

### GET - Retrieve Pizza by ID

**Request:**

```http
GET http://localhost:5149/pizza/3
Accept: application/json
```

**HTTP Status:** 200 OK

**Response:**

```json
{
	"id": 3,
	"name": "Mexican Fiesta",
	"isGlutenFree": false
}
```

### GET - Pizza Not Found

**Request:**

```http
GET http://localhost:5149/pizza/99
Accept: application/json
```

**HTTP Status:** 404 Not Found

**Response (excluding the generated trace ID):**

```json
{
	"type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
	"title": "Not Found",
	"status": 404
}
```

The actual response also included a generated `traceId`.

### POST - Create a New Pizza

**Request:**

```http
POST http://localhost:5149/pizza/
Content-Type: application/json

{
  "name": "Hawaii",
  "isGlutenFree": false
}
```

**HTTP Status:** 201 Created

**Location Header:** `http://localhost:5149/Pizza/4`

**Response:**

```json
{
	"id": 4,
	"name": "Hawaii",
	"isGlutenFree": false
}
```

The POST request created a pizza named Hawaii and assigned it ID 4.

### PUT - Update an Existing Pizza

**Request:**

```http
PUT http://localhost:5149/pizza/4
Content-Type: application/json

{
  "id": 4,
  "name": "Hawaiian",
  "isGlutenFree": false
}
```

**HTTP Status:** 204 No Content

**Response:** No response body.

### GET - Verify the Update

**Request:**

```http
GET http://localhost:5149/pizza/4
Accept: application/json
```

**HTTP Status:** 200 OK

**Response:**

```json
{
	"id": 4,
	"name": "Hawaiian",
	"isGlutenFree": false
}
```

This confirmed that the pizza name changed from Hawaii to Hawaiian.

### DELETE - Remove a Pizza

**Request:**

```http
DELETE http://localhost:5149/pizza/4
```

**HTTP Status:** 204 No Content

**Response:** No response body.

### GET - Verify the Deletion

**Request:**

```http
GET http://localhost:5149/pizza/
Accept: application/json
```

**HTTP Status:** 200 OK

**Response:**

```json
[
	{
		"id": 1,
		"name": "Classic Italian",
		"isGlutenFree": false
	},
	{
		"id": 2,
		"name": "Veggie",
		"isGlutenFree": true
	},
	{
		"id": 3,
		"name": "Mexican Fiesta",
		"isGlutenFree": false
	}
]
```

This confirmed that Hawaiian was deleted. The three seeded pizzas remained.

## Module 5 - Sales Summary

For Module 5, I added a `WriteSalesSummary()` function that reads the sales JSON files, calculates the overall total and each file's subtotal, and writes a report.

### Additional Function: WriteSalesSummary()

```csharp
void WriteSalesSummary(IEnumerable<string> salesFiles, string reportPath)
{
    decimal grandTotal = 0;
    List<string> details = new List<string>();

    foreach (var file in salesFiles)
    {
        if (Path.GetFileName(file) != "sales.json")
        {
            continue;
        }

        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        decimal fileTotal = data == null ? 0 : (decimal)data.Total;
        grandTotal += fileTotal;

        string fileName = Path.GetRelativePath(
            Directory.GetCurrentDirectory(), file);

        details.Add($"{fileName}: {fileTotal:C2}");
    }

    string report =
        "Sales Summary" + Environment.NewLine +
        "-------------" + Environment.NewLine +
        $"Total Sales: {grandTotal:C2}" + Environment.NewLine +
        Environment.NewLine +
        "Details:" + Environment.NewLine +
        string.Join(Environment.NewLine, details);

    File.WriteAllText(reportPath, report);
}
```

### Generated Sales Summary Report

The function generated `Module05-Files-Directories/mslearn-dotnet-files/salesTotalDir/sales-summary.txt` with these results:

```text
Sales Summary
-------------
Total Sales: $2,012.20

Details:
stores\sales.json: $88.88
stores\201\sales.json: $501.22
stores\202\sales.json: $1,234.22
stores\203\sales.json: $99.00
stores\204\sales.json: $88.88
```

The report shows total sales of $2,012.20 and a subtotal for each sales file.
