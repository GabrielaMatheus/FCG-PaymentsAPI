using FiapCloudGames.PaymentsApi.Consumers;
using FiapCloudGames.PaymentsApi.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPaymentSimulator, PaymentSimulator>();

builder.Services.AddMassTransit(bus =>
{
    bus.AddConsumer<OrderPlacedConsumer>();
    bus.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"] ?? "localhost", "/", host =>
        {
            host.Username(builder.Configuration["RabbitMq:Username"] ?? "guest");
            host.Password(builder.Configuration["RabbitMq:Password"] ?? "guest");
        });

        cfg.ReceiveEndpoint("payments-order-placed", endpoint =>
        {
            endpoint.UseMessageRetry(retry => retry.Intervals(1000, 5000, 15000));
            endpoint.ConfigureConsumer<OrderPlacedConsumer>(context);
        });
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapGet("/", () => Results.Ok(new { service = "FCG PaymentsAPI", status = "running" }));
app.MapHealthChecks("/health");
app.Run();

public partial class Program;
