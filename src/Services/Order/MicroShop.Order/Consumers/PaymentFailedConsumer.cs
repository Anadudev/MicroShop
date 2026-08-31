using MassTransit;
using MicroShop.Contracts.Payments;
using MicroShop.Order.Data;
using MicroShop.Order.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Order.Consumers;

public class PaymentFailedConsumer(OrderDbContext db):IConsumer<PaymentFailed>
{
    public async Task Consume(ConsumeContext<PaymentFailed> context)
    {
        var message = context.Message;
        var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == message.OrderId);
        if (order == null)
            return;
        order.Status = OrderStatus.Canceled;
        await db.SaveChangesAsync();
    }
}
