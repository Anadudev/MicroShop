using MicroShop.Order.Clients;
using MicroShop.Order.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderDatabase")));
builder.Services.AddHttpClient<ProductClient>(client =>
{
    client.BaseAddress =
        new Uri(builder.Configuration["ProductService:BaseUrl"] ?? throw new InvalidOperationException());
});

builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();