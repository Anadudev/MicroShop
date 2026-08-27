namespace MicroShop.Contracts.Orders;

public record OrderCreated(
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    DateTime CreatedAt
);
