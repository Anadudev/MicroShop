using MassTransit;
using MicroShop.Notification.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("notification", false));
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConnectionString = builder.Configuration.GetConnectionString("rabbitmq");
        if (!string.IsNullOrEmpty(rabbitMqConnectionString))
        {
            cfg.Host(new Uri(rabbitMqConnectionString));
        }
        else
        {
            cfg.Host(
                "localhost",
                "/",
                h =>
                {
                    h.Username("microshop");
                    h.Password("microshop");
                });
        }

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

app.MapDefaultEndpoints();

app.Run();
