namespace FiapCloudGames.PaymentsApi.Services;

public interface IPaymentSimulator
{
    string Process(Guid orderId, decimal price);
}

// Simulacao deterministica: o resultado depende apenas do OrderId, entao o
// mesmo pedido sempre produz o mesmo resultado (facilita demonstrar e testar
// o fluxo sem depender de aleatoriedade real).
public class PaymentSimulator : IPaymentSimulator
{
    private const int PercentualAprovacao = 80;

    public string Process(Guid orderId, decimal price)
    {
        var hash = unchecked((uint)orderId.GetHashCode());
        var bucket = hash % 100;

        return bucket < PercentualAprovacao ? "Approved" : "Rejected";
    }
}
