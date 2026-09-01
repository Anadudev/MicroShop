using Microsoft.EntityFrameworkCore;

namespace MicroShop.Order.Data;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<Models.Order> Orders => Set<Models.Order>();
    public DbSet<Models.OrderItem> OrdersItems => Set<Models.OrderItem>();
}
