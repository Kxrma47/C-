

# Shop Management System

This is a **Shop Management System** built with C# and .NET. The system allows for the management of goods, buyers, shops, and sales transactions. It includes functionalities for data access, entity management, serialization, and database operations, making it a flexible and scalable solution for small-scale shop or e-commerce management.

## Features

- **Goods Management**: 
  - Create, retrieve, and manage products (goods) sold by the shop.
  - Track product categories, shop IDs, and prices.
  
- **Sales Management**: 
  - Track sales transactions, including buyer details, shop information, and the quantity of goods sold.
  
- **Buyer Management**: 
  - Manage buyer information, including name, surname, city, and country.

- **Shop Management**: 
  - Maintain a list of shops, with details such as name, city, and country.

- **Database Operations**:
  - Perform basic CRUD (Create, Read, Update, Delete) operations on various entities (goods, buyers, shops, and sales).
  - Serialize and deserialize data to/from JSON files for persistence.

- **Advanced Data Queries**:
  - Retrieve goods bought by the buyer with the longest name.
  - Find the most expensive goods and their categories.
  - Get data about sales in various cities and countries.
  - Calculate the total value of sales.

- **Error Handling**:
  - Custom database-related exception handling with `DataBaseException`.

## Project Structure

- **Entities**: 
  - `Good`: Represents a product sold by the shop.
  - `Sale`: Represents a sale transaction.
  - `Buyer`: Represents a customer who makes purchases.
  - `Shop`: Represents a physical or online store.
  
- **Interfaces**:
  - `IEntity`: Base interface that all entities (goods, buyers, sales, shops) implement.
  - `IDataBase`: Interface for handling database operations like creating tables, inserting data, serializing, and deserializing.
  - `IDataAccessLayer`: Interface for performing complex queries like retrieving the most expensive goods, or buyers of popular products.

- **Error Handling**:
  - `DataBaseException`: A custom exception class for handling errors related to database operations.

### Usage

The system allows you to manage basic shop operations:

- **Goods**: Add new products with a name, category, and price.
- **Sales**: Record sales transactions, specifying buyer, product, and quantity.
- **Buyers**: Track buyers' personal details and their purchase history.
- **Shops**: Manage the list of shops and their respective locations.

You can also serialize data to JSON files and retrieve it later for persistence between sessions.

### Example

```csharp
var db = new DataBase();

// Create tables
db.CreateTable<Good>();
db.CreateTable<Buyer>();

// Insert data
db.InsertInto<Good>(() => new Good("Laptop", 1, "Electronics", 1500));
db.InsertInto<Buyer>(() => new Buyer("John", "Doe", "New York", "USA"));

// Serialize to JSON
db.Serialize<Good>("goods.json");
db.Serialize<Buyer>("buyers.json");

// Deserialize and print data
db.Deserialize<Good>("goods.json");
db.Deserialize<Buyer>("buyers.json");
```

### Advanced Data Queries

The `IDataAccessLayer` interface provides methods for advanced queries, such as retrieving the most popular goods or calculating the total value of sales:

```csharp
var dataAccessLayer = new DataAccessLayer();
var expensiveCategory = dataAccessLayer.GetMostExpensiveGoodCategory(db);
Console.WriteLine($"Most Expensive Good Category: {expensiveCategory}");
```
