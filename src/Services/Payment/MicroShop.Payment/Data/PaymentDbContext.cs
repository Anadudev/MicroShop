using Microsoft.EntityFrameworkCore;

namespace MicroShop.Payment.Data;

public class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : DbContext(options)
{
    public DbSet<Models.Payment> Payments => Set<Models.Payment>();
}
