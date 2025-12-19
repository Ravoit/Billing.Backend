using System.Collections.Concurrent;
using Backend.Gateways;
using Backend.Models;

namespace Backend.Services;

public sealed class OrderService(ILogger<IOrderService> logger, IEnumerable<IPaymentGateway> gateways) : IOrderService
{
    private static readonly ConcurrentDictionary<string, PaymentReceipt?> ProcessedOrders = [];

    public async Task<IResult> ProcessOrderAsync(OrderRequest order)
    {
        if (string.IsNullOrEmpty(order.OrderNumber))
        {
            var error = new OrderError("No order number provided.");

            logger.LogError(error.Error);
            return Results.BadRequest(error);
        }
        
        logger.LogInformation("Processing order '{OrderNumber}'.", order.OrderNumber);

        if (!ProcessedOrders.TryAdd(order.OrderNumber, null))
        {
            var error = new OrderError($"Order '{order.OrderNumber}' already processed.");

            logger.LogError(error.Error);
            return Results.BadRequest(error);
        }

        var gateway = gateways.FirstOrDefault(g => g.Id == order.PaymentGatewayId);
        if (gateway == null)
        {
            var error = new OrderError($"Payment gateway '{order.PaymentGatewayId}' not found.");

            logger.LogError(error.Error);
            return Results.BadRequest(error);
        }

        var result = await gateway.ProcessPaymentAsync(order);

        if (!result.Success)
        {
            var error = new OrderError(result.ErrorMessage ?? "Unknown payment error.");

            logger.LogError(error.Error);
            return Results.BadRequest(error);
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