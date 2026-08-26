using MicroShop.Product.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Product.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(ProductDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.Product>>> GetProducts()
    {
        var products = await db.Products.AsNoTracking().ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Models.Product>> GetProduct(Guid id)
    {
        var product = await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Models.Product>> CreateProduct(Models.Product product)
    {
        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;

        db.Products.Add(product);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, Models.Product updatedProduct)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        product.Name = updatedProduct.Name;
        product.Description = updatedProduct.Description;
        product.Price = updatedProduct.Price;
        product.StockQuantity = updatedProduct.StockQuantity;

        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        db.Products.Remove(product);
        await db.SaveChangesAsync();

        return NoContent();
    }
}
