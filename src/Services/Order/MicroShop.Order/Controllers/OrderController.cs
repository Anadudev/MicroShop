using MassTransit;
using MicroShop.Contracts.Orders;
using MicroShop.Order.Clients;
using MicroShop.Order.Data;
using MicroShop.Order.DTOs;
using MicroShop.Order.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Order.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(OrderDbContext db, ProductClient productClient, IPublishEndpoint publishEndpoint) : ControllerBase
{
    private readonly OrderDbContext _db = db;
    private readonly ProductClient _productClient = productClient;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    [HttpPost]
    public async Task<ActionResult<Models.Order>> CreateOrder(CreateOrderRequest request)
    {
        var order = new Models.Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            Status = Models.OrderStatus.Pending
        };

        foreach (var item in request.Items)
        {
            var product = await _productClient.GetProductAsync(item.ProductId);
            if (product is null)
            {
                return BadRequest($"Product {item.ProductId} not found");
            }

            if (product.StockQuantity < item.Quantity)
            {
                return BadRequest($"Not enough stock for product");
            }

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            };
            order.items.Add(orderItem);
        }

        order.TotalAmount = order.items.Sum(item => item.UnitPrice * item.Quantity);
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        await _publishEndpoint.Publish(
            new OrderCreated(
                order.Id,
                order.CustomerId,
                order.TotalAmount,
                order.CreatedAt
            ));
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IEnumerable<Models.Order>>> GetOrder(Guid id)
    {
        var order = await _db.Orders.Include(order => order.items).FirstOrDefaultAsync(order => order.Id == id);

        if (order is null)
            return NotFound();

        return Ok(order);
    }
}
