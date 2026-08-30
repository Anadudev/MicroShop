using MassTransit;
using MicroShop.Payment.Consumers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("payment", false));
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
