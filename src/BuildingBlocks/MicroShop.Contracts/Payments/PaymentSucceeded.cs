namespace MicroShop.Contracts.Payments;

public record PaymentSucceeded(
    Guid OrderId,
    Guid PaymentId,
    decimal Amount,
    DateTime ProcessedAt
);
