using Microsoft.EntityFrameworkCore;

namespace MicroShop.Product.Data;

/// <summary>
/// Represents the database context for the Product service, providing access to the Products DbSet and configuring the database connection using Entity Framework Core.
/// </summary>
/// <param name="options"></param>
public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Models.Product> Products => Set<Models.Product>();
}