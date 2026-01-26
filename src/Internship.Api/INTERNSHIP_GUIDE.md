# Internship Preparation Guide: Database Concepts & Entity Framework Core

This comprehensive guide covers all the essential concepts you need for your internship session, demonstrated through a complete API project with Entity Framework Core and SQLite.

## Table of Contents
1. [Introduction to Databases](#introduction-to-databases)
2. [Entity Framework Core Basics](#entity-framework-core-basics)
3. [Database Relationships & Advanced Queries](#database-relationships--advanced-queries)
4. [Project Structure](#project-structure)
5. [Running the Project](#running-the-project)
6. [API Endpoints](#api-endpoints)
7. [Key Concepts Summary](#key-concepts-summary)

---

## Introduction to Databases

### Database Concepts: Tables, Rows, Columns, Keys

#### Tables
A **table** is a collection of related data organized in rows and columns. Think of it like a spreadsheet.

```sql
-- Example: Users table
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT UNIQUE NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

#### Rows (Records)
Each **row** represents a single record or entity instance.

```sql
-- Example rows in Users table
INSERT INTO Users (FirstName, LastName, Email) VALUES 
('John', 'Doe', 'john@example.com'),
('Jane', 'Smith', 'jane@example.com');
```

#### Columns (Fields)
Each **column** represents a specific attribute or property of the entity.

- `Id` - Primary key (unique identifier)
- `FirstName` - User's first name
- `LastName` - User's last name
- `Email` - User's email address

#### Keys

**Primary Key**: Uniquely identifies each row
```sql
Id INTEGER PRIMARY KEY
```

**Foreign Key**: Links to another table's primary key
```sql
UserId INTEGER REFERENCES Users(Id)
```

**Unique Key**: Ensures no duplicate values
```sql
Email TEXT UNIQUE
```

### SQL Basics: SELECT, INSERT, UPDATE, DELETE

#### SELECT - Retrieving Data
```sql
-- Basic select
SELECT * FROM Users;

-- Select specific columns
SELECT FirstName, LastName FROM Users;

-- Filter with WHERE
SELECT * FROM Users WHERE IsActive = 1;

-- Sort with ORDER BY
SELECT * FROM Users ORDER BY LastName, FirstName;

-- Limit results
SELECT * FROM Users LIMIT 10;
```

#### INSERT - Adding Data
```sql
-- Insert single record
INSERT INTO Users (FirstName, LastName, Email) 
VALUES ('John', 'Doe', 'john@example.com');

-- Insert multiple records
INSERT INTO Users (FirstName, LastName, Email) VALUES 
('Jane', 'Smith', 'jane@example.com'),
('Bob', 'Johnson', 'bob@example.com');
```

#### UPDATE - Modifying Data
```sql
-- Update specific record
UPDATE Users 
SET FirstName = 'Johnny' 
WHERE Id = 1;

-- Update multiple columns
UPDATE Users 
SET FirstName = 'Johnny', LastName = 'Doe Jr.' 
WHERE Id = 1;
```

#### DELETE - Removing Data
```sql
-- Delete specific record
DELETE FROM Users WHERE Id = 1;

-- Delete with conditions
DELETE FROM Users WHERE IsActive = 0;
```

### SQLite Setup and Usage

SQLite is a lightweight, file-based database perfect for development and learning.

#### Advantages of SQLite:
- **Zero Configuration**: No server setup required
- **File-based**: Database stored in a single file
- **Cross-platform**: Works on Windows, Mac, Linux
- **ACID Compliant**: Reliable data integrity
- **Embedded**: No separate database server needed

#### Connection String:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=internship.db"
  }
}
```

### Database Design Principles

#### 1. Normalization
Organize data to reduce redundancy and improve integrity.

**First Normal Form (1NF)**: Each column contains atomic values
**Second Normal Form (2NF)**: No partial dependencies
**Third Normal Form (3NF)**: No transitive dependencies

#### 2. Relationships
- **One-to-One**: Each record in Table A relates to one record in Table B
- **One-to-Many**: Each record in Table A can relate to many records in Table B
- **Many-to-Many**: Records in Table A can relate to many records in Table B, and vice versa

#### 3. Indexing
Create indexes on frequently queried columns for better performance.

```sql
-- Create index on email column
CREATE INDEX IX_Users_Email ON Users(Email);

-- Create composite index
CREATE INDEX IX_Orders_User_Date ON Orders(UserId, OrderDate);
```

---

## Entity Framework Core Basics

### ORM Concepts & Benefits

**Object-Relational Mapping (ORM)** is a programming technique that allows you to work with databases using object-oriented programming concepts.

#### Benefits:
- **Productivity**: Write less SQL code
- **Type Safety**: Compile-time checking
- **Maintainability**: Easier to modify database schema
- **Database Agnostic**: Switch between different databases
- **LINQ Support**: Query using familiar C# syntax

### EF Core Setup

#### 1. Install NuGet Packages
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
```

#### 2. Create DbContext
```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
}
```

#### 3. Configure in Program.cs
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### Code-First Approach

**Code-First** means you write your entity classes first, then EF Core generates the database schema.

#### Entity Class Example:
```csharp
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

#### Data Annotations:
- `[Key]` - Primary key
- `[Required]` - Not null constraint
- `[MaxLength(n)]` - Maximum string length
- `[EmailAddress]` - Email validation
- `[Column(TypeName = "decimal(10,2)")]` - Column type specification

### DbContext & DbSet Basics (CRUD Operations)

#### DbContext
The main class that coordinates Entity Framework functionality for a data model.

#### DbSet
Represents a collection of entities that can be queried from the database.

#### CRUD Operations:

**Create (Insert)**
```csharp
var user = new User 
{ 
    FirstName = "John", 
    LastName = "Doe", 
    Email = "john@example.com" 
};
_context.Users.Add(user);
await _context.SaveChangesAsync();
```

**Read (Select)**
```csharp
// Get all users
var users = await _context.Users.ToListAsync();

// Get user by ID
var user = await _context.Users.FindAsync(1);

// Get with conditions
var activeUsers = await _context.Users
    .Where(u => u.IsActive)
    .ToListAsync();
```

**Update**
```csharp
var user = await _context.Users.FindAsync(1);
if (user != null)
{
    user.FirstName = "Johnny";
    await _context.SaveChangesAsync();
}
```

**Delete**
```csharp
var user = await _context.Users.FindAsync(1);
if (user != null)
{
    _context.Users.Remove(user);
    await _context.SaveChangesAsync();
}
```

---

## Database Relationships & Advanced Queries

### One-to-Many & Many-to-Many Relationships

#### One-to-Many Relationship
One user can have many orders.

```csharp
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    
    // Navigation property
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign key
    public decimal TotalAmount { get; set; }
    
    // Navigation property
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
```

#### Many-to-Many Relationship
Users can have multiple roles, and roles can be assigned to multiple users.

```csharp
// Junction table
public class UserRole
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    
    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}

public class User
{
    public int Id { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
```

### Navigation Properties

Navigation properties allow you to navigate between related entities.

```csharp
// Eager loading - load related data upfront
var usersWithOrders = await _context.Users
    .Include(u => u.Orders)
    .ToListAsync();

// Multiple levels of includes
var usersWithOrderDetails = await _context.Users
    .Include(u => u.Orders)
        .ThenInclude(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
    .ToListAsync();
```

### Eager vs Lazy Loading

#### Eager Loading
Load related data immediately with the main entity.

```csharp
// Explicit eager loading
var users = await _context.Users
    .Include(u => u.Orders)
    .ToListAsync();
```

#### Lazy Loading
Load related data when accessed (requires virtual properties).

```csharp
// Enable lazy loading in DbContext
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseLazyLoadingProxies();
}

// Lazy loading usage
var user = await _context.Users.FindAsync(1);
var orders = user.Orders; // Loaded automatically when accessed
```

### Advanced LINQ Queries

#### Filtering
```csharp
// Basic filtering
var activeUsers = await _context.Users
    .Where(u => u.IsActive)
    .ToListAsync();

// Multiple conditions
var recentUsers = await _context.Users
    .Where(u => u.IsActive && u.CreatedAt >= DateTime.UtcNow.AddDays(-30))
    .ToListAsync();

// String operations
var gmailUsers = await _context.Users
    .Where(u => u.Email.Contains("@gmail.com"))
    .ToListAsync();
```

#### Aggregation
```csharp
// Count
var totalUsers = await _context.Users.CountAsync();
var activeUsersCount = await _context.Users.CountAsync(u => u.IsActive);

// Sum
var totalRevenue = await _context.Orders
    .Where(o => o.Status != "Cancelled")
    .SumAsync(o => o.TotalAmount);

// Average
var averageOrderValue = await _context.Orders
    .AverageAsync(o => o.TotalAmount);

// Group by
var categoryStats = await _context.Products
    .GroupBy(p => p.Category)
    .Select(g => new
    {
        Category = g.Key,
        Count = g.Count(),
        AveragePrice = g.Average(p => p.Price)
    })
    .ToListAsync();
```

#### Sorting and Pagination
```csharp
// Sorting
var products = await _context.Products
    .OrderBy(p => p.Category)
    .ThenByDescending(p => p.Price)
    .ToListAsync();

// Pagination
int pageNumber = 1;
int pageSize = 10;
var pagedProducts = await _context.Products
    .OrderBy(p => p.Name)
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

#### Joins
```csharp
// Using navigation properties (recommended)
var ordersWithUsers = await _context.Orders
    .Include(o => o.User)
    .ToListAsync();

// Manual joins
var orderDetails = await _context.Orders
    .Join(_context.Users,
        order => order.UserId,
        user => user.Id,
        (order, user) => new
        {
            OrderId = order.Id,
            CustomerName = user.FullName,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount
        })
    .ToListAsync();
```

---

## Project Structure

```
InternshipAPI/
├── Controllers/
│   ├── UsersController.cs
│   ├── ProductsController.cs
│   └── OrdersController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   ├── User.cs
│   ├── Product.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   ├── Role.cs
│   └── UserRole.cs
├── Services/
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── IProductService.cs
│   ├── ProductService.cs
│   ├── IOrderService.cs
│   └── OrderService.cs
├── Examples/
│   └── AdvancedQueriesExample.cs
├── Program.cs
├── appsettings.json
└── InternshipAPI.csproj
```

---

## Running the Project

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Steps to Run

1. **Clone/Download the project**
2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access Swagger UI**
   - Navigate to `https://localhost:7000/swagger` (or the URL shown in console)
   - Explore the API endpoints

5. **Database Creation**
   - The database will be created automatically on first run
   - Seed data will be inserted automatically

---

## API Endpoints

### Users API
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user (soft delete)
- `GET /api/users/with-orders` - Get users with orders (eager loading)
- `GET /api/users/by-role/{roleName}` - Get users by role
- `GET /api/users/{id}/total-spent` - Get user's total spending

### Products API
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product (soft delete)
- `GET /api/products/category/{category}` - Get products by category
- `GET /api/products/low-stock` - Get low stock products
- `GET /api/products/average-price` - Get average product price

### Orders API
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `POST /api/orders` - Create new order
- `PUT /api/orders/{id}` - Update order
- `DELETE /api/orders/{id}` - Delete order
- `GET /api/orders/user/{userId}` - Get orders by user
- `GET /api/orders/status/{status}` - Get orders by status
- `GET /api/orders/revenue` - Get total revenue
- `GET /api/orders/statistics` - Get order statistics

---

## Key Concepts Summary

### Database Fundamentals
- **Tables**: Collections of related data
- **Rows**: Individual records
- **Columns**: Data attributes
- **Keys**: Primary (unique ID) and Foreign (references)
- **SQL**: Standard language for database operations

### Entity Framework Core
- **ORM**: Object-Relational Mapping for easier database work
- **Code-First**: Write C# classes, generate database schema
- **DbContext**: Main class for database operations
- **DbSet**: Collection of entities for querying

### Relationships
- **One-to-Many**: User has many Orders
- **Many-to-Many**: Users have multiple Roles
- **Navigation Properties**: Easy traversal between related entities

### Query Techniques
- **LINQ**: Language Integrated Query for type-safe database queries
- **Eager Loading**: Load related data upfront with Include()
- **Lazy Loading**: Load related data when accessed
- **Projection**: Select only needed data for performance

### Best Practices
- Use meaningful entity and property names
- Implement proper validation with data annotations
- Use async/await for database operations
- Implement soft deletes for data preservation
- Use navigation properties instead of manual joins
- Consider performance with large datasets

This guide provides a solid foundation for your internship session. Practice with the provided examples and experiment with the API endpoints to reinforce your understanding!
