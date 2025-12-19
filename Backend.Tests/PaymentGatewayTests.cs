using Backend.Gateways;
using Backend.Gateways.Implementations;
using Backend.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace Backend.Tests;

[TestClass]
public sealed class PaymentGatewayTests
{
    [TestMethod]
    public async Task ProcessPayment_InvalidAmount_ReturnsPaymentFailed()
    {
        var paymentGateway = new BankPaymentGateway(new NullLogger<BankPaymentGateway>());

        var order = new OrderRequest("ORDER-1", "USER-1", 0, "Bank");

        var result = await paymentGateway.ProcessPaymentAsync(order);

        Assert.IsInstanceOfType<PaymentResult>(result);
        Assert.IsFalse(result.Success);
    }

    [TestMethod]
    public async Task ProcessPayment_ReturnsPaymentSuccess()
    {
        var paymentGateway = new BankPaymentGateway(new NullLogger<BankPaymentGateway>());

        var order = new OrderRequest("ORDER-1", "USER-1", 1, "Bank");

        var result = await paymentGateway.ProcessPaymentAsync(order);

        Assert.IsInstanceOfType<PaymentResult>(result);
        Assert.IsTrue(result.Success);
    }
}