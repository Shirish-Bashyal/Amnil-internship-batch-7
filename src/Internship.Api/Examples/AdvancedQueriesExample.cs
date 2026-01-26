using Microsoft.EntityFrameworkCore;
using Internship.Api.Data;
using Internship.Api.Models;

namespace Internship.Api.Examples
{
    /// <summary>
    /// Advanced LINQ Queries Examples
    /// This class demonstrates various advanced LINQ operations with Entity Framework Core
    /// </summary>
    public class AdvancedQueriesExample
    {
        private readonly ApplicationDbContext _context;

        public AdvancedQueriesExample(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 1. FILTERING QUERIES
        /// Demonstrates various filtering techniques
        /// </summary>
        public async Task<IEnumerable<User>> FilteringExamples()
        {
            // Basic filtering
            var activeUsers = await _context.Users
                .Where(u => u.IsActive)
                .ToListAsync();

            // Multiple conditions
            var recentUsers = await _context.Users
                .Where(u => u.IsActive && u.CreatedAt >= DateTime.UtcNow.AddDays(-30))
                .ToListAsync();

            // String operations
            var usersWithGmail = await _context.Users
                .Where(u => u.Email.Contains("@gmail.com"))
                .ToListAsync();

            // Null checks
            var usersWithNotes = await _context.Users
                .Where(u => !string.IsNullOrEmpty(u.FirstName))
                .ToListAsync();

            // Range queries
            var expensiveProducts = await _context.Products
                .Where(p => p.Price >= 100 && p.Price <= 1000)
                .ToListAsync();

            return activeUsers;
        }

        /// <summary>
        /// 2. SORTING AND PAGINATION
        /// Demonstrates ordering and paging techniques
        /// </summary>
        public async Task<IEnumerable<Product>> SortingAndPaginationExamples()
        {
            // Simple sorting
            var productsByName = await _context.Products
                .OrderBy(p => p.Name)
                .ToListAsync();

            // Multiple sorting criteria
            var productsByCategoryAndPrice = await _context.Products
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

            return productsByName;
        }

        /// <summary>
        /// 3. AGGREGATION QUERIES
        /// Demonstrates various aggregation functions
        /// </summary>
        public async Task<object> AggregationExamples()
        {
            // Count
            var totalUsers = await _context.Users.CountAsync();
            var activeUsersCount = await _context.Users.CountAsync(u => u.IsActive);

            // Sum
            var totalRevenue = await _context.Orders
                .Where(o => o.Status != "Cancelled")
                .SumAsync(o => o.TotalAmount);

            // Average
            var averageOrderValue = await _context.Orders
                .Where(o => o.Status != "Cancelled")
                .AverageAsync(o => o.TotalAmount);

            // Min/Max
            var cheapestProduct = await _context.Products
                .Where(p => p.IsActive)
                .MinAsync(p => p.Price);

            var mostExpensiveProduct = await _context.Products
                .Where(p => p.IsActive)
                .MaxAsync(p => p.Price);

            // Group by with aggregation
            var categoryStats = await _context.Products
                .Where(p => p.IsActive)
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count(),
                    AveragePrice = g.Average(p => p.Price),
                    TotalStock = g.Sum(p => p.StockQuantity)
                })
                .ToListAsync();

            return new
            {
                TotalUsers = totalUsers,
                ActiveUsersCount = activeUsersCount,
                TotalRevenue = totalRevenue,
                AverageOrderValue = averageOrderValue,
                CheapestProduct = cheapestProduct,
                MostExpensiveProduct = mostExpensiveProduct,
                CategoryStats = categoryStats
            };
        }

        /// <summary>
        /// 4. JOIN OPERATIONS
        /// Demonstrates various join techniques
        /// </summary>
        public async Task<IEnumerable<object>> JoinExamples()
        {
            // Inner join using navigation properties
            var usersWithOrders = await _context.Users
                .Where(u => u.Orders.Any())
                .Select(u => new
                {
                    UserId = u.Id,
                    UserName = u.FullName,
                    OrderCount = u.Orders.Count,
                    TotalSpent = u.Orders.Sum(o => o.TotalAmount)
                })
                .ToListAsync();

            // Manual join using Join method
            var orderDetails = await _context.Orders
                .Join(_context.Users,
                    order => order.UserId,
                    user => user.Id,
                    (order, user) => new
                    {
                        OrderId = order.Id,
                        order.OrderNumber,
                        CustomerName = user.FullName,
                        order.OrderDate,
                        order.TotalAmount
                    })
                .ToListAsync();

            // Multiple joins
            var detailedOrderInfo = await _context.Orders
                .Join(_context.Users, o => o.UserId, u => u.Id, (o, u) => new { Order = o, User = u })
                .Join(_context.OrderItems, ou => ou.Order.Id, oi => oi.OrderId, (ou, oi) => new { ou.Order, ou.User, OrderItem = oi })
                .Join(_context.Products, oui => oui.OrderItem.ProductId, p => p.Id, (oui, p) => new
                {
                    oui.Order.OrderNumber,
                    CustomerName = oui.User.FullName,
                    ProductName = p.Name,
                    oui.OrderItem.Quantity,
                    oui.OrderItem.UnitPrice,
                    oui.OrderItem.LineTotal
                })
                .ToListAsync();

            return usersWithOrders;
        }

        /// <summary>
        /// 5. EAGER VS LAZY LOADING
        /// Demonstrates different loading strategies
        /// </summary>
        public async Task<IEnumerable<Order>> LoadingStrategiesExamples()
        {
            // Eager Loading - Load related data upfront
            var ordersWithDetails = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();

            // Selective Eager Loading - Load only specific related data
            var ordersWithUserOnly = await _context.Orders
                .Include(o => o.User)
                .ToListAsync();

            // Projection - Load only specific fields
            var orderSummaries = await _context.Orders
                .Select(o => new
                {
                    OrderId = o.Id,
                    o.OrderNumber,
                    CustomerName = o.User.FullName,
                    o.OrderDate,
                    o.TotalAmount,
                    ItemCount = o.OrderItems.Count
                })
                .ToListAsync();

            return ordersWithDetails;
        }

        /// <summary>
        /// 6. COMPLEX QUERIES WITH SUBQUERIES
        /// Demonstrates complex query patterns
        /// </summary>
        public async Task<IEnumerable<object>> ComplexQueryExamples()
        {
            // Subquery in WHERE clause
            var usersWithHighValueOrders = await _context.Users
                .Where(u => u.Orders.Any(o => o.TotalAmount > 500))
                .Select(u => new
                {
                    UserId = u.Id,
                    UserName = u.FullName,
                    MaxOrderValue = u.Orders.Max(o => o.TotalAmount)
                })
                .ToListAsync();

            // Subquery in SELECT clause
            var productSalesInfo = await _context.Products
                .Select(p => new
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    TotalSold = _context.OrderItems
                        .Where(oi => oi.ProductId == p.Id)
                        .Sum(oi => oi.Quantity),
                    TotalRevenue = _context.OrderItems
                        .Where(oi => oi.ProductId == p.Id)
                        .Sum(oi => oi.LineTotal)
                })
                .ToListAsync();

            // EXISTS subquery
            var productsInOrders = await _context.Products
                .Where(p => _context.OrderItems.Any(oi => oi.ProductId == p.Id))
                .ToListAsync();

            return usersWithHighValueOrders;
        }

        /// <summary>
        /// 7. WINDOW FUNCTIONS AND RANKING
        /// Demonstrates advanced SQL concepts using LINQ
        /// </summary>
        public async Task<IEnumerable<object>> WindowFunctionExamples()
        {
            // Top N per category
            var topProductsPerCategory = await _context.Products
                .GroupBy(p => p.Category)
                .SelectMany(g => g.OrderByDescending(p => p.Price).Take(2))
                .Select(p => new
                {
                    p.Category,
                    ProductName = p.Name,
                    p.Price
                })
                .ToListAsync();

            // Ranking within groups
            var rankedProducts = await _context.Products
                .OrderBy(p => p.Category)
                .ThenByDescending(p => p.Price)
                .Select((p, index) => new
                {
                    Rank = index + 1,
                    p.Category,
                    ProductName = p.Name,
                    p.Price
                })
                .ToListAsync();

            return topProductsPerCategory;
        }

        /// <summary>
        /// 8. BULK OPERATIONS
        /// Demonstrates bulk insert, update, and delete operations
        /// </summary>
        public async Task BulkOperationExamples()
        {
            // Bulk insert
            var newProducts = new List<Product>
            {
                new Product { Name = "Product 1", Price = 10.99m, StockQuantity = 100, Category = "Electronics" },
                new Product { Name = "Product 2", Price = 20.99m, StockQuantity = 50, Category = "Electronics" },
                new Product { Name = "Product 3", Price = 30.99m, StockQuantity = 25, Category = "Books" }
            };

            _context.Products.AddRange(newProducts);
            await _context.SaveChangesAsync();

            // Bulk update
            await _context.Products
                .Where(p => p.Category == "Electronics")
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.Price, x => x.Price * 1.1m));

            // Bulk delete (soft delete)
            await _context.Products
                .Where(p => p.StockQuantity == 0)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsActive, false));
        }

        /// <summary>
        /// 9. RAW SQL QUERIES
        /// Demonstrates how to execute raw SQL when needed
        /// </summary>
        public async Task<IEnumerable<object>> RawSqlExamples()
        {
            // Raw SQL query
            var rawQueryResult = await _context.Database
                .SqlQueryRaw<object>(@"
                    SELECT 
                        u.FirstName + ' ' + u.LastName as FullName,
                        COUNT(o.Id) as OrderCount,
                        SUM(o.TotalAmount) as TotalSpent
                    FROM Users u
                    LEFT JOIN Orders o ON u.Id = o.UserId
                    WHERE u.IsActive = 1
                    GROUP BY u.Id, u.FirstName, u.LastName
                    HAVING COUNT(o.Id) > 0
                    ORDER BY TotalSpent DESC")
                .ToListAsync();

            // Raw SQL with parameters
            var parameterizedQuery = await _context.Database
                .SqlQueryRaw<object>(@"
                    SELECT * FROM Products 
                    WHERE Category = {0} AND Price > {1}",
                    "Electronics", 50.00m)
                .ToListAsync();

            return rawQueryResult;
        }

        /// <summary>
        /// 10. PERFORMANCE OPTIMIZATION TECHNIQUES
        /// Demonstrates query optimization strategies
        /// </summary>
        public async Task<IEnumerable<object>> PerformanceOptimizationExamples()
        {
            // Use AsNoTracking for read-only queries
            var readOnlyData = await _context.Users
                .AsNoTracking()
                .Where(u => u.IsActive)
                .Select(u => new { u.Id, u.FullName, u.Email })
                .ToListAsync();

            // Use compiled queries for frequently executed queries
            var compiledQuery = EF.CompileQuery((ApplicationDbContext context, int userId) =>
                context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => new { u.Id, u.FullName, u.Email })
                    .FirstOrDefault());

            var user = compiledQuery(_context, 1);

            // Use projection to load only needed data
            var projectedData = await _context.Orders
                .Select(o => new
                {
                    o.Id,
                    o.OrderNumber,
                    o.OrderDate,
                    o.TotalAmount,
                    CustomerName = o.User.FullName,
                    ItemCount = o.OrderItems.Count
                })
                .ToListAsync();

            return readOnlyData;
        }
    }
}
