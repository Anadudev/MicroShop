using MassTransit;
using MicroShop.Contracts.Orders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(
            "localhost",
            "/",
            h =>
            {
                h.Username("microshop");
                h.Password("microshop");
            });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
app.Run();
