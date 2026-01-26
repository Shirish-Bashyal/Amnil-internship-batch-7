# Internship Preparation: Database & EF Core API

A comprehensive example API demonstrating database concepts, Entity Framework Core, and advanced LINQ queries to help you prepare for your internship session.

## 🎯 What This Project Covers

### Database Concepts
- Tables, Rows, Columns, Keys
- SQL Basics (SELECT, INSERT, UPDATE, DELETE)
- SQLite Setup and Usage
- Database Design Principles

### Entity Framework Core
- ORM Concepts & Benefits
- EF Core Setup and Configuration
- Code-First Approach
- DbContext & DbSet Basics
- CRUD Operations

### Advanced Topics
- One-to-Many & Many-to-Many Relationships
- Navigation Properties
- Eager vs Lazy Loading
- Advanced LINQ Queries
- Filtering, Aggregation, Grouping
- Performance Optimization

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Running the Project

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
   - Navigate to `https://localhost:7000/swagger`
   - Explore the API endpoints

5. **Database**
   - SQLite database (`internship.db`) will be created automatically
   - Seed data will be inserted on first run

## 📁 Project Structure

```
InternshipAPI/
├── Controllers/          # API Controllers
├── Data/                # DbContext and configuration
├── Models/              # Entity models
├── Services/            # Business logic services
├── Examples/            # Advanced query examples
├── Program.cs           # Application entry point
└── appsettings.json     # Configuration
```

## 🗄️ Database Schema

### Entities
- **User**: Customer information
- **Product**: Product catalog
- **Order**: Customer orders
- **OrderItem**: Items within orders
- **Role**: User roles
- **UserRole**: Many-to-many junction table

### Relationships
- User → Orders (One-to-Many)
- Order → OrderItems (One-to-Many)
- Product → OrderItems (One-to-Many)
- User ↔ Roles (Many-to-Many via UserRole)

## 🔧 Key Features

### CRUD Operations
- Complete CRUD for Users, Products, and Orders
- Soft delete implementation
- Validation with data annotations

### Advanced Queries
- Eager loading with Include()
- Complex filtering and sorting
- Aggregation queries (Count, Sum, Average)
- Group by operations
- Pagination support

### API Endpoints

#### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user
- `GET /api/users/with-orders` - Users with orders (eager loading)
- `GET /api/users/by-role/{roleName}` - Users by role
- `GET /api/users/{id}/total-spent` - User's total spending

#### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product
- `GET /api/products/category/{category}` - Products by category
- `GET /api/products/low-stock` - Low stock products
- `GET /api/products/average-price` - Average price

#### Orders
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `POST /api/orders` - Create order
- `PUT /api/orders/{id}` - Update order
- `DELETE /api/orders/{id}` - Delete order
- `GET /api/orders/user/{userId}` - Orders by user
- `GET /api/orders/status/{status}` - Orders by status
- `GET /api/orders/revenue` - Total revenue
- `GET /api/orders/statistics` - Order statistics

## 📚 Learning Resources

### Database Concepts
- Tables, rows, columns, and keys
- SQL basics and best practices
- Database design principles
- Normalization and relationships

### Entity Framework Core
- ORM concepts and benefits
- Code-first approach
- DbContext and DbSet usage
- Data annotations and configuration

### Advanced Queries
- LINQ to Entities
- Eager vs Lazy loading
- Complex joins and filtering
- Performance optimization techniques

## 🧪 Testing the API

### Using Swagger UI
1. Run the application
2. Navigate to `/swagger`
3. Try the different endpoints
4. View request/response schemas

### Using curl/Postman
```bash
# Get all users
curl -X GET "https://localhost:7000/api/users"

# Create a new user
curl -X POST "https://localhost:7000/api/users" \
  -H "Content-Type: application/json" \
  -d '{"firstName":"John","lastName":"Doe","email":"john@example.com"}'

# Get users with orders
curl -X GET "https://localhost:7000/api/users/with-orders"
```

## 📖 Documentation

See `INTERNSHIP_GUIDE.md` for comprehensive documentation covering:
- Detailed explanations of all concepts
- Code examples and best practices
- SQL query examples
- Entity Framework Core patterns
- Advanced LINQ techniques

## 🎓 Study Tips

1. **Start with the basics**: Understand tables, rows, columns, and keys
2. **Practice SQL**: Try the SQL examples in the guide
3. **Explore the models**: Look at the entity classes and their relationships
4. **Test the API**: Use Swagger to understand how data flows
5. **Study the services**: See how business logic is implemented
6. **Review the examples**: Check `AdvancedQueriesExample.cs` for complex queries

## 🤝 Contributing

This is a learning project. Feel free to:
- Add more examples
- Improve documentation
- Add more test cases
- Suggest improvements

## 📄 License

This project is for educational purposes. Use it to learn and prepare for your internship!

