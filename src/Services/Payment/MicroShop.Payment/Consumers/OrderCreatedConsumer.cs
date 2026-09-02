using MassTransit;
using MicroShop.Contracts.Orders;
using MicroShop.Contracts.Payments;
using MicroShop.Payment.Data;
using MicroShop.Payment.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Payment.Consumers;

public class OrderCreatedConsumer(PaymentDbContext db, IPublishEndpoint publishEndpoint)
    : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var order = context.Message;
        var messageId = context.MessageId ?? throw new InvalidOperationException("Message Id is required");
        var alreadyProcessed = await db.ProcessedMessages.AnyAsync(m => m.MessageId == messageId.ToString());
        if (alreadyProcessed)
        {
            Console.WriteLine($"Message {messageId} already processed.");
            return;
        }

        var payment = new Models.Payment
        {
            Id = Guid.NewGuid(),
            OrderId = order.OrderId,
            Amount = order.TotalAmount,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        db.Payments.Add(payment);
        db.ProcessedMessages.Add(
            new ProcessedMessage
            {
                Id = Guid.NewGuid(),
                MessageId = messageId.ToString(),
            });
        await db.SaveChangesAsync();

        // Simulate payment processing
        const bool successful = true;
        if (successful)
        {
            payment.Status = Models.PaymentStatus.Succeeded;
            await db.SaveChangesAsync();
            await publishEndpoint.Publish(
                new PaymentSucceeded(
                    order.OrderId, payment.Id, order.TotalAmount,
                    DateTime.UtcNow
                ));
        }
        else
        {
            payment.Status = Models.PaymentStatus.Failed;
            await publishEndpoint.Publish(
                new PaymentFailed(
                    order.OrderId, payment.Id, order.TotalAmount,
                    payment.FailureReason ?? "",
                    DateTime.UtcNow
                ));
        }

        await Task.CompletedTask;
    }
}
