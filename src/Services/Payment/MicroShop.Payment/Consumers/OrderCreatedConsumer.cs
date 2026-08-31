using MassTransit;
using MicroShop.Contracts.Orders;
using MicroShop.Contracts.Payments;
using MicroShop.Payment.Data;

namespace MicroShop.Payment.Consumers;

public class OrderCreatedConsumer(PaymentDbContext db, IPublishEndpoint publishEndpoint)
    : IConsumer<OrderCreated>
{
    private readonly PaymentDbContext _db = db;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var order = context.Message;
        var payment = new Models.Payment
        {
            Id = Guid.NewGuid(),
            OrderId = order.OrderId,
            Amount = order.TotalAmount,
            Status = Models.PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        // Simulate payment processing
        var successful = true;
        if (successful)
        {
            payment.Status = Models.PaymentStatus.Succeeded;
            await _db.SaveChangesAsync();
            await _publishEndpoint.Publish(
                new PaymentSucceeded(
                    order.OrderId, payment.Id, order.TotalAmount,
                    DateTime.UtcNow
                ));
        }
        else
        {
            payment.Status = Models.PaymentStatus.Failed;
            await _publishEndpoint.Publish(
                new PaymentFailed(
                    order.OrderId, payment.Id, order.TotalAmount,
                    payment.FailureReason ?? "",
                    DateTime.UtcNow
                ));
        }

        await Task.CompletedTask;
    }
}
