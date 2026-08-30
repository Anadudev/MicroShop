using MassTransit;
using MicroShop.Contracts.Orders;

namespace MicroShop.Payment.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var message = context.Message;
        // Handle the OrderCreated event here
        Console.WriteLine($"Processing payment for Order ID: {message.OrderId},\nAmount: {message.TotalAmount},\nCustomer ID: {message.CustomerId}");

        await Task.CompletedTask;
    }
}
