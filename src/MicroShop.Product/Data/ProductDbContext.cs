using Microsoft.EntityFrameworkCore;
using MicroShop.Product.Models;

namespace MicroShop.Product.Data;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Models.Product> Products => Set<Models.Product>();
}