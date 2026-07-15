using FiapCloudGames.Contracts;
using FiapCloudGames.PaymentsApi.Services;
using MassTransit;

namespace FiapCloudGames.PaymentsApi.Consumers;

public class OrderPlacedConsumer(IPaymentSimulator paymentSimulator, ILogger<OrderPlacedConsumer> logger)
    : IConsumer<OrderPlacedEvent>
{
    public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
    {
        var pedido = context.Message;

        logger.LogInformation(
            "OrderPlacedEvent recebido: pedido {OrderId}, jogo {GameName}, valor {Price}",
            pedido.OrderId, pedido.GameName, pedido.Price);

        var status = paymentSimulator.Process(pedido.OrderId, pedido.Price);

        logger.LogInformation("Pagamento do pedido {OrderId} simulado como {Status}", pedido.OrderId, status);

        await context.Publish(new PaymentProcessedEvent(
            pedido.OrderId,
            pedido.UserId,
            pedido.GameId,
            pedido.UserEmail,
            pedido.GameName,
            pedido.Price,
            status,
            DateTime.UtcNow));
    }
}
