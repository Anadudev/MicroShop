namespace MicroShop.Order.DTOs;

public record CreateOrderRequest(
    Guid CustomerId,
    List<CreateOrderItemRequest> Items
);

public record CreateOrderItemRequest(
    Guid ProductId,
    int Quantity
);
