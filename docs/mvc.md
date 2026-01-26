# Comprehensive MVC Guide for .NET Developers

## Table of Contents
1. [ASP.NET MVC Fundamentals](#aspnet-mvc-fundamentals)
2. [Project Structure & Setup](#project-structure--setup)
3. [Controllers Deep Dive](#controllers-deep-dive)
4. [Views & Razor Syntax](#views--razor-syntax)
5. [Models & Data Annotations](#models--data-annotations)
6. [Entity Framework Integration](#entity-framework-integration)
7. [Dependency Injection](#dependency-injection)
8. [Advanced Topics](#advanced-topics)
9. [Best Practices](#best-practices)

## ASP.NET MVC Fundamentals

### What is ASP.NET MVC?
ASP.NET MVC is a web application framework developed by Microsoft that implements the Model-View-Controller pattern. It provides an alternative to Web Forms for building scalable, testable web applications.

### Key Components in .NET MVC

```csharp
// Traditional MVC Flow in .NET
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews(); // Register MVC services
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
```

## Project Structure & Setup

### Typical .NET MVC Project Structure
```
MyAspNetApp/
├── Controllers/
│   ├── HomeController.cs
│   ├── ProductController.cs
│   └── AccountController.cs
├── Models/
│   ├── Product.cs
│   ├── User.cs
│   └── ViewModels/
├── Views/
│   ├── Home/
│   ├── Product/
│   ├── Shared/
│   └── _ViewStart.cshtml
├── wwwroot/
├── Data/
├── Services/
└── Program.cs
```

### Creating a New MVC Project

#### Using .NET CLI
```bash
dotnet new mvc -n MyAspNetApp
cd MyAspNetApp
dotnet run
```

#### Using Visual Studio
1. File → New → Project
2. Select "ASP.NET Core Web App (Model-View-Controller)"
3. Configure authentication and other options

## Controllers Deep Dive

### Basic Controller Structure
```csharp
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
```

### Action Results and Return Types
```csharp
public class ProductController : Controller
{
    // View Result - Renders a view
    public IActionResult Index()
    {
        return View();
    }

    // View with Model
    public IActionResult Details(int id)
    {
        var product = _productService.GetProduct(id);
        return View(product);
    }

    // JSON Result
    public IActionResult GetProductJson(int id)
    {
        var product = _productService.GetProduct(id);
        return Json(product);
    }

    // Redirect
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            _productService.AddProduct(product);
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // File Result
    public IActionResult DownloadReport()
    {
        var bytes = System.IO.File.ReadAllBytes("report.pdf");
        return File(bytes, "application/pdf", "report.pdf");
    }

    // Content Result
    public IActionResult PlainText()
    {
        return Content("Hello World", "text/plain");
    }

    // Status Code Results
    public IActionResult NotFoundResult()
    {
        return NotFound();
    }

    public IActionResult UnauthorizedResult()
    {
        return Unauthorized();
    }
}
```

### Action Filters and Attributes
```csharp
// Custom Action Filter
public class LogActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Logic before action executes
        Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} is executing");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Logic after action executes
        Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} executed");
    }
}

// Using Action Filters
[ServiceFilter(typeof(LogActionFilter))]
[Authorize] // Authentication filter
[ValidateAntiForgeryToken] // Security filter
[RequireHttps] // Security filter
[ResponseCache(Duration = 60)] // Caching filter
public class SecureController : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SensitiveOperation()
    {
        // Protected action
        return View();
    }
}
```

### Parameter Binding
```csharp
public class UserController : Controller
{
    // From Route: /user/details/5
    public IActionResult Details(int id) // Bound from route
    {
        return View();
    }

    // From Query: /user/search?name=john
    public IActionResult Search([FromQuery] string name)
    {
        return View();
    }

    // From Form
    [HttpPost]
    public IActionResult Create([FromForm] User user)
    {
        return RedirectToAction(nameof(Index));
    }

    // From Body (JSON/XML)
    [HttpPost]
    public IActionResult Update([FromBody] User user)
    {
        return Ok();
    }

    // From Header
    public IActionResult GetFromHeader([FromHeader] string userAgent)
    {
        return Content($"User Agent: {userAgent}");
    }

    // From Services (Dependency Injection)
    public IActionResult GetWithService([FromServices] IUserService userService)
    {
        var users = userService.GetAll();
        return View(users);
    }
}
```

## Views & Razor Syntax

### Razor Syntax Basics
```html
@* Views/Product/Index.cshtml *@
@model List<Product>

@{
    ViewData["Title"] = "Products";
    Layout = "_Layout";
}

<h2>@ViewData["Title"]</h2>

@* Conditional rendering *@
@if (Model.Any())
{
    <table class="table">
        <thead>
            <tr>
                <th>Name</th>
                <th>Price</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            @* Looping *@
            @foreach (var product in Model)
            {
                <tr>
                    <td>@product.Name</td>
                    <td>@product.Price.ToString("C")</td>
                    <td>
                        @* Action links *@
                        <a asp-action="Details" asp-route-id="@product.Id" class="btn btn-info">Details</a>
                        <a asp-action="Edit" asp-route-id="@product.Id" class="btn btn-warning">Edit</a>
                    </td>
                </tr>
            }
        </tbody>
    </table>
}
else
{
    <p>No products found.</p>
}

@* Partial views *@
<partial name="_ProductStats" />

@* HTML Helpers (legacy but still used) *@
@Html.ActionLink("Create New", "Create")
```

### Strongly-Typed Views and View Models
```csharp
// View Model
public class ProductViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Product Name")]
    [Required(ErrorMessage = "Product name is required")]
    public string Name { get; set; }
    
    [DataType(DataType.Currency)]
    [Range(0.01, 1000.00, ErrorMessage = "Price must be between 0.01 and 1000.00")]
    public decimal Price { get; set; }
    
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    public List<SelectListItem> Categories { get; set; }
    
    [Display(Name = "Description")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; }
}
```

```html
@* Views/Product/Create.cshtml *@
@model ProductViewModel

@{
    ViewData["Title"] = "Create Product";
}

<h2>Create New Product</h2>

<form asp-action="Create" method="post">
    <div class="form-group">
        <label asp-for="Name" class="control-label"></label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>
    
    <div class="form-group">
        <label asp-for="Price" class="control-label"></label>
        <input asp-for="Price" class="form-control" />
        <span asp-validation-for="Price" class="text-danger"></span>
    </div>
    
    <div class="form-group">
        <label asp-for="CategoryId" class="control-label"></label>
        <select asp-for="CategoryId" asp-items="Model.Categories" class="form-control">
            <option value="">-- Select Category --</option>
        </select>
        <span asp-validation-for="CategoryId" class="text-danger"></span>
    </div>
    
    <div class="form-group">
        <label asp-for="Description" class="control-label"></label>
        <textarea asp-for="Description" class="form-control" rows="4"></textarea>
        <span asp-validation-for="Description" class="text-danger"></span>
    </div>
    
    <div class="form-group">
        <input type="submit" value="Create" class="btn btn-primary" />
        <a asp-action="Index" class="btn btn-secondary">Cancel</a>
    </div>
</form>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

### Layouts and Sections
```html
@* Views/Shared/_Layout.cshtml *@
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - My App</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" />
    @await RenderSectionAsync("Styles", required: false)
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">My App</a>
                <partial name="_LoginPartial" />
            </div>
        </nav>
    </header>
    
    <div class="container">
        <main role="main" class="pb-3">
            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2024 - My App - <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
        </div>
    </footer>

    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

```html
@* Using sections in views *@
@section Styles {
    <style>
        .custom-style { color: blue; }
    </style>
}

@section Scripts {
    <script>
        $(document).ready(function() {
            console.log('Page loaded');
        });
    </script>
}
```

### Tag Helpers
```html
@* Common Tag Helpers *@
<!-- Form Tag Helper -->
<form asp-controller="Product" asp-action="Create" method="post">
    <!-- Input Tag Helper -->
    <input asp-for="Name" class="form-control" />
    
    <!-- Label Tag Helper -->
    <label asp-for="Price" class="control-label"></label>
    
    <!-- Validation Tag Helper -->
    <span asp-validation-for="Description" class="text-danger"></span>
    
    <!-- Select Tag Helper -->
    <select asp-for="CategoryId" asp-items="Model.Categories"></select>
    
    <!-- Anchor Tag Helper -->
    <a asp-controller="Home" asp-action="Index">Home</a>
    <a asp-route-id="5" asp-route-name="test">Link with parameters</a>
</form>

<!-- Environment Tag Helper -->
<environment include="Development">
    <script src="~/js/dev-scripts.js"></script>
</environment>
<environment exclude="Development">
    <script src="~/js/prod-scripts.min.js"></script>
</environment>

<!-- Cache Tag Helper -->
<cache expires-after="@TimeSpan.FromMinutes(10)">
    <div>This content will be cached for 10 minutes</div>
</cache>
```

## Models & Data Annotations

### Data Models with Validation
```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters")]
    [Display(Name = "Product Name")]
    public string Name { get; set; }
    
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00")]
    public decimal Price { get; set; }
    
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }
    
    [Display(Name = "Created Date")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    [Display(Name = "Last Updated")]
    [DataType(DataType.DateTime)]
    public DateTime? LastUpdated { get; set; }
    
    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    // Custom validation method
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Price < 0)
        {
            yield return new ValidationResult(
                "Price cannot be negative", 
                new[] { nameof(Price) });
        }
    }
}

// Custom validation attribute
public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime date)
        {
            return date > DateTime.Now;
        }
        return false;
    }
}
```

### View Models for Specific Use Cases
```csharp
// Create View Model
public class ProductCreateViewModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public List<SelectListItem> AvailableCategories { get; set; }
}

// Edit View Model
public class ProductEditViewModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public List<SelectListItem> AvailableCategories { get; set; }
}

// List View Model
public class ProductListViewModel
{
    public List<Product> Products { get; set; }
    public string SearchTerm { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;
}
```

## Entity Framework Integration

### DbContext and Repository Pattern
```csharp
// Application DbContext
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure entity relationships and constraints
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name)
            .IsUnique();

        // Seed data
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Books" },
            new Category { Id = 3, Name = "Clothing" }
        );
    }
}

// Generic Repository
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveAsync();
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    
    public void Update(T entity) => _dbSet.Update(entity);
    
    public void Delete(T entity) => _dbSet.Remove(entity);
    
    public async Task SaveAsync() => await _context.SaveChangesAsync();
}
```

### Service Layer with Business Logic
```csharp
// Product Service
public interface IProductService
{
    Task<Product> GetProductByIdAsync(int id);
    Task<List<Product>> GetAllProductsAsync();
    Task CreateProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<List<Product>> SearchProductsAsync(string searchTerm);
    Task<bool> ProductExistsAsync(int id);
}

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IRepository<Product> productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        try
        {
            return await _productRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with ID {ProductId}", id);
            throw;
        }
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return (await _productRepository.GetAllAsync()).ToList();
    }

    public async Task CreateProductAsync(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        // Business logic validation
        if (product.Price <= 0)
            throw new InvalidOperationException("Product price must be greater than zero");

        await _productRepository.AddAsync(product);
        await _productRepository.SaveAsync();
        
        _logger.LogInformation("Product {ProductName} created with ID {ProductId}", 
            product.Name, product.Id);
    }

    public async Task UpdateProductAsync(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        var existingProduct = await _productRepository.GetByIdAsync(product.Id);
        if (existingProduct == null)
            throw new KeyNotFoundException($"Product with ID {product.Id} not found");

        // Update properties
        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.Description = product.Description;
        existingProduct.CategoryId = product.CategoryId;
        existingProduct.LastUpdated = DateTime.UtcNow;

        _productRepository.Update(existingProduct);
        await _productRepository.SaveAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found");

        _productRepository.Delete(product);
        await _productRepository.SaveAsync();
        
        _logger.LogInformation("Product with ID {ProductId} deleted", id);
    }

    public async Task<List<Product>> SearchProductsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllProductsAsync();

        var allProducts = await _productRepository.GetAllAsync();
        return allProducts
            .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public async Task<bool> ProductExistsAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product != null;
    }
}
```

## Dependency Injection

### Configuration in Program.cs
```csharp
// Program.cs (NET 6+)
var builder = WebApplication.CreateBuilder(args);

// Add services to DI container
builder.Services.AddControllersWithViews();

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Custom Services
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// HTTP Client
builder.Services.AddHttpClient();

// Configuration
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### Controller with Dependency Injection
```csharp
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IProductService productService,
        ICategoryService categoryService,
        ILogger<ProductController> logger)
    {
        _productService = productService;
        _categoryService = categoryService;
        _logger = logger;
    }

    // GET: Product
    public async Task<IActionResult> Index(string searchTerm, int page = 1)
    {
        try
        {
            var products = string.IsNullOrEmpty(searchTerm) 
                ? await _productService.GetAllProductsAsync()
                : await _productService.SearchProductsAsync(searchTerm);

            var model = new ProductListViewModel
            {
                Products = products,
                SearchTerm = searchTerm,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(products.Count / 10.0)
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products");
            return View("Error");
        }
    }

    // GET: Product/Create
    public async Task<IActionResult> Create()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        var model = new ProductCreateViewModel
        {
            AvailableCategories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList()
        };

        return View(model);
    }

    // POST: Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Repopulate categories if validation fails
            var categories = await _categoryService.GetAllCategoriesAsync();
            model.AvailableCategories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            
            return View(model);
        }

        try
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                CategoryId = model.CategoryId
            };

            await _productService.CreateProductAsync(product);
            
            TempData["SuccessMessage"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            ModelState.AddModelError("", "An error occurred while creating the product.");
            
            // Repopulate categories
            var categories = await _categoryService.GetAllCategoriesAsync();
            model.AvailableCategories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            
            return View(model);
        }
    }
}
```

## Advanced Topics

### Custom Model Binders
```csharp
// Custom model binder for complex types
public class ProductModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var model = new Product();
        
        // Bind properties from various sources
        model.Name = bindingContext.ValueProvider.GetValue("Name").FirstValue;
        
        if (decimal.TryParse(bindingContext.ValueProvider.GetValue("Price").FirstValue, out decimal price))
        {
            model.Price = price;
        }
        
        model.Description = bindingContext.ValueProvider.GetValue("Description").FirstValue;
        
        if (int.TryParse(bindingContext.ValueProvider.GetValue("CategoryId").FirstValue, out int categoryId))
        {
            model.CategoryId = categoryId;
        }

        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }
}

// Register custom model binder
[ModelBinder(BinderType = typeof(ProductModelBinder))]
public class Product
{
    // Properties...
}
```

### Custom Action Filters
```csharp
// Custom action filter for logging
public class LogActionAttribute : ActionFilterAttribute
{
    private readonly ILogger _logger;

    public LogActionAttribute(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<LogActionAttribute>();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} is executing");
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} executed");
        base.OnActionExecuted(context);
    }
}

// Custom authorization filter
public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _requiredRoles;

    public CustomAuthorizeAttribute(params string[] roles)
    {
        _requiredRoles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        if (_requiredRoles.Any() && !_requiredRoles.Any(user.IsInRole))
        {
            context.Result = new ForbidResult();
        }
    }
}

// Using custom filters
[LogAction]
[CustomAuthorize("Admin", "Manager")]
public class AdminController : Controller
{
    // Controller actions...
}
```

### API Controllers
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsApiController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsApiController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        
        if (product == null)
            return NotFound();
            
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _productService.UpdateProductAsync(product);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
```

## Best Practices

### 1. Keep Controllers Thin
```csharp
// ❌ Bad - Fat controller
public class BadProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public async Task<IActionResult> Create(Product product)
    {
        // Business logic in controller
        if (product.Price < 0)
        {
            ModelState.AddModelError("Price", "Price cannot be negative");
        }

        // Data access in controller
        if (await _context.Products.AnyAsync(p => p.Name == product.Name))
        {
            ModelState.AddModelError("Name", "Product name already exists");
        }

        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View(product);
    }
}

// ✅ Good - Thin controller
public class GoodProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public async Task<IActionResult> Create(ProductCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategories(model);
            return View(model);
        }

        var result = await _productService.CreateProductAsync(model);
        
        if (result.Success)
        {
            TempData["Success"] = "Product created successfully";
            return RedirectToAction(nameof(Index));
        }

        // Add service errors to model state
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Key, error.Value);
        }

        await PopulateCategories(model);
        return View(model);
    }

    private async Task PopulateCategories(ProductCreateViewModel model)
    {
        model.AvailableCategories = await _categoryService.GetCategorySelectListAsync();
    }
}
```

### 2. Proper Error Handling
```csharp
// Global exception handling
public class CustomExceptionFilter : IExceptionFilter
{
    private readonly ILogger<CustomExceptionFilter> _logger;
    private readonly IWebHostEnvironment _env;

    public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "An unhandled exception occurred");

        if (context.Exception is KeyNotFoundException)
        {
            context.Result = new NotFoundObjectResult(new { error = "Resource not found" });
        }
        else if (context.Exception is UnauthorizedAccessException)
        {
            context.Result = new UnauthorizedResult();
        }
        else
        {
            var error = _env.IsDevelopment() 
                ? new { error = context.Exception.Message, stackTrace = context.Exception.StackTrace }
                : new { error = "An unexpected error occurred" };
                
            context.Result = new ObjectResult(error) { StatusCode = 500 };
        }

        context.ExceptionHandled = true;
    }
}

// Service result pattern
public class ServiceResult<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public Dictionary<string, string> Errors { get; set; } = new();

    public static ServiceResult<T> Ok(T data) => new() { Success = true, Data = data };
    public static ServiceResult<T> Fail(string error) => new() { Success = false, Errors = { [""] = error } };
}
```

### 3. Security Best Practices
```csharp
// Secure controller with proper attributes
[Authorize]
[AutoValidateAntiforgeryToken]
[RequireHttps]
public class SecureController : Controller
{
    private readonly IProductService _productService;

    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> AdminOnlyAction()
    {
        // Only accessible by Admin or Manager roles
        return View();
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> SecureForm(Product model)
    {
        // Protected against CSRF
        return View();
    }

    [AllowAnonymous]
    public IActionResult PublicAction()
    {
        // Accessible without authentication
        return View();
    }
}

// Input sanitization
public class SanitizeInputAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is string stringValue)
            {
                // Sanitize string inputs
                var sanitized = SanitizeHtml(stringValue);
                // Update the value...
            }
        }
        
        base.OnActionExecuting(context);
    }

    private string SanitizeHtml(string html)
    {
        // Implement HTML sanitization logic
        return html; // Simplified
    }
}
```
