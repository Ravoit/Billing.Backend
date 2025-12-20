using Billing.Backend.Gateways.Implementations;
using Billing.Backend.Models;
using Billing.Backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging.Abstractions;

namespace Billing.Backend.Tests;

[TestClass]
public sealed class OrderTests
{
    private readonly OrderRequest _order = new("ORDER-1", "USER-1", 1, "Bank");

    [TestMethod]
    public async Task ProcessOrder_NoGateway_ReturnsBadRequest()
    {
        var orderService = new OrderService(new NullLogger<OrderService>(), []);

        var result = await orderService.ProcessOrderAsync(_order);

        Assert.IsInstanceOfType<BadRequest<OrderError>>(result);
    }

    [TestMethod]
    public async Task ProcessOrder_ReturnsOk()
    {
        var orderService = new OrderService(new NullLogger<OrderService>(), [
            new BankPaymentGateway(new NullLogger<BankPaymentGateway>())
        ]);

        var result = await orderService.ProcessOrderAsync(_order);

        Assert.IsInstanceOfType<Ok<PaymentReceipt>>(result);
    }

    [TestMethod]
    public async Task ProcessOrder_DoubleOrder_ReturnsBadRequest()
    {
        var orderService = new OrderService(new NullLogger<OrderService>(), [
            new BankPaymentGateway(new NullLogger<BankPaymentGateway>())
        ]);

        await orderService.ProcessOrderAsync(_order);
        var result = await orderService.ProcessOrderAsync(_order);

        Assert.IsInstanceOfType<BadRequest<OrderError>>(result);
    }
}