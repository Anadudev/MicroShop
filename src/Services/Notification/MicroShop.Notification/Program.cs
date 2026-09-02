using MassTransit;
using MicroShop.Notification.Consumers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("notification", false));
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

        cfg.UseMessageRetry(retry =>
        {
            retry.Interval(
                3,
                TimeSpan.FromSeconds(5));
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
app.Run();
