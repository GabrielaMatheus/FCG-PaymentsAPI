using FiapCloudGames.PaymentsApi.Services;
using Xunit;

namespace FiapCloudGames.PaymentsApi.Tests;

public class PaymentSimulatorTests
{
    [Fact]
    public void Process_deve_ser_deterministico_para_o_mesmo_pedido()
    {
        var simulator = new PaymentSimulator();
        var orderId = Guid.NewGuid();

        var resultado1 = simulator.Process(orderId, 100m);
        var resultado2 = simulator.Process(orderId, 100m);

        Assert.Equal(resultado1, resultado2);
    }

    [Fact]
    public void Process_deve_retornar_apenas_Approved_ou_Rejected()
    {
        var simulator = new PaymentSimulator();

        for (var i = 0; i < 50; i++)
        {
            var status = simulator.Process(Guid.NewGuid(), 100m);
            Assert.True(status is "Approved" or "Rejected");
        }
    }
}
