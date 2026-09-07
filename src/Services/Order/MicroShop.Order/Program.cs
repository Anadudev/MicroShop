using MassTransit;
using MicroShop.Order.Clients;
using MicroShop.Order.Consumers;
using MicroShop.Order.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderDatabase")));
builder.Services.AddHttpClient<ProductClient>(client =>
{
    client.BaseAddress =
        new Uri(builder.Configuration["ProductService:BaseUrl"] ?? throw new InvalidOperationException());
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentFailedConsumer>();
    x.AddConsumer<PaymentSucceededConsumer>();
    x.AddEntityFrameworkOutbox<OrderDbContext>(o =>
    {
        o.UsePostgres();
        o.UseBusOutbox();
    });
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConnectionString = builder.Configuration.GetConnectionString("rabbitmq");
        if (!string.IsNullOrEmpty(rabbitMqConnectionString))
        {
            cfg.Host(new Uri(rabbitMqConnectionString));
        }
        else
        {
            cfg.Host("localhost", "/", h =>
            {
                h.Username("microshop");
                h.Password("microshop");
            });
        }
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
