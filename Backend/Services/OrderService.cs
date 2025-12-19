using System.Collections.Concurrent;
using Backend.Gateways;
using Backend.Models;

namespace Backend.Services;

public sealed class OrderService(ILogger<IOrderService> logger, IEnumerable<IPaymentGateway> gateways) : IOrderService
{
    private static readonly ConcurrentDictionary<string, PaymentReceipt?> ProcessedOrders = [];

    public async Task<IResult> ProcessOrderAsync(OrderRequest order)
    {
        logger.LogInformation("Processing order '{OrderNumber}'.", order.OrderNumber);

        if (!ProcessedOrders.TryAdd(order.OrderNumber, null))
        {
            return Results.BadRequest(new { Error = $"Order '{order.OrderNumber}' already processed." });
        }

        var gateway = gateways.FirstOrDefault(g => g.Id == order.PaymentGatewayId);
        if (gateway == null)
        {
            logger.LogError("Payment gateway '{PaymentGatewayId}' not found.", order.PaymentGatewayId);
            return Results.BadRequest(new { Error = $"Payment gateway '{order.PaymentGatewayId}' not found." });
        }

        var result = await gateway.ProcessPaymentAsync(order);

        if (!result.Success)
        {
            return Results.BadRequest(new { Error = result.ErrorMessage ?? "Unknown payment error." });
        }

        var receipt = new PaymentReceipt(
            order.OrderNumber,
            order.Amount,
            DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            result.ConfirmationId!
        );

        ProcessedOrders[order.OrderNumber] = receipt;
        return Results.Ok(receipt);
    }
}