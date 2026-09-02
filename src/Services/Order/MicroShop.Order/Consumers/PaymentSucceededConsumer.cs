using MassTransit;
using MicroShop.Contracts.Payments;
using MicroShop.Order.Data;
using MicroShop.Order.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Order.Consumers;

public class PaymentSucceededConsumer(OrderDbContext db) : IConsumer<PaymentSucceeded>
{
    public async Task Consume(ConsumeContext<PaymentSucceeded> context)
    {
     var   message = context.Message;
     var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == message.OrderId);
     
     if (order == null)
              return;
     
     order.Status = OrderStatus.Confirmed;
     await db.SaveChangesAsync();
    }
}