using System.Net.Http.Json;

namespace MicroShop.Order.Clients;

public class ProductClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ProductResponse?> GetProductAsync(Guid productId)
    {
        return await _httpClient.GetFromJsonAsync<ProductResponse>($"/api/products/{productId}");
    }
}

public record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    int StockQuantity
);