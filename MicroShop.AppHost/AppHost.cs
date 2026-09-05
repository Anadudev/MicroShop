var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL container hosting isolated databases for Product, Order, and Payment services
var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5433)
    .WithPgAdmin()
    .WithDataVolume();

var productDb = postgres.AddDatabase("ProductDatabase", "microshop_products");
var orderDb = postgres.AddDatabase("OrderDatabase", "microshop_orders");
var paymentDb = postgres.AddDatabase("PaymentDatabase", "microshop_payments");

// RabbitMQ message broker with management UI
var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithDataVolume()
    .WithManagementPlugin();

// Product Service
var product = builder.AddProject<Projects.MicroShop_Product>("product")
    .WithReference(productDb)
    .WaitFor(productDb)
    .WithExternalHttpEndpoints();

// Order Service
var order = builder.AddProject<Projects.MicroShop_Order>("order")
    .WithReference(orderDb)
    .WaitFor(orderDb)
    .WithReference(product)
    .WaitFor(product)
    .WithEnvironment("ProductService__BaseUrl", product.GetEndpoint("http"))
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithExternalHttpEndpoints();

// Payment Service
var payment = builder.AddProject<Projects.MicroShop_Payment>("payment")
    .WithReference(paymentDb)
    .WaitFor(paymentDb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);

// Notification Service
var notification = builder.AddProject<Projects.MicroShop_Notification>("notification")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);

builder.Build().Run();
