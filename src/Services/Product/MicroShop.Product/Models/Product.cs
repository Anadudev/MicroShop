namespace MicroShop.Product.Models;

/// <summary>
/// Represents a product in the system with properties such as Id, Name, Description, Price, StockQuantity, and CreatedAt.
/// </summary>
public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
