using MassTransit;
using MicroShop.Contracts.Payments;
using MicroShop.Order.Data;
using MicroShop.Order.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Order.Consumers;

public class PaymentFailedConsumer(OrderDbContext db) : IConsumer<PaymentFailed>
{
    public async Task Consume(ConsumeContext<PaymentFailed> context)
    {
        var message = context.Message;
        Console.WriteLine($"Processing order created {message.OrderId}");
        // throw new Exception("Simulated Payment Failure");
        var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == message.OrderId);
        // Console.WriteLine($"Payment failed for order {message.OrderId}, reason: {message.Reason}");
        if (order == null)
            return;
        order.Status = OrderStatus.Canceled;
        await db.SaveChangesAsync();
    }
}
// 
