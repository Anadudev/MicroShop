using MassTransit;
using MicroShop.Payment.Consumers;
using MicroShop.Payment.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PaymentDatabase")));
builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("payment", false));
    x.AddConsumer<OrderCreatedConsumer>();
    x.AddEntityFrameworkOutbox<PaymentDbContext>(o =>
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
            retry.Interval(3,
                TimeSpan.FromSeconds(5));
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
