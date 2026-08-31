namespace MicroShop.Contracts.Payments;


public record PaymentFailed(
    Guid OrderId,
    Guid PaymentId,
    decimal Amount,
    string Reason,
    DateTime ProcessedAt
);
