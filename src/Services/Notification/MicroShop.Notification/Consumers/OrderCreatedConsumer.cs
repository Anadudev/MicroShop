using MassTransit;
using MicroShop.Contracts.Orders;

namespace MicroShop.Notification.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var message = context.Message;
        // Handle the OrderCreated event here
        Console.WriteLine($"Order Created: {message.OrderId} \n Customer: {message.CustomerId} \n Total Amount: {message.TotalAmount} \n Created At: {message.CreatedAt}");

        await Task.CompletedTask;
    }
}
