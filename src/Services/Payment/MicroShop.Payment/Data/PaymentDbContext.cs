using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Payment.Data;

public class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : DbContext(options)
{
    public DbSet<Models.Payment> Payments => Set<Models.Payment>();
    public DbSet<Models.ProcessedMessage> ProcessedMessages => Set<Models.ProcessedMessage>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Models.ProcessedMessage>()
            .HasIndex(p => p.MessageId).IsUnique();
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
