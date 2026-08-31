namespace MicroShop.Payment.Models;

public class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string? FailureReason { get; set; }
    public DateTime CreatedAt { get; set; }
}
