using Backend.Models;

namespace Backend.Gateways.Implementations;

public class MockPaymentGateway(string id, ILogger logger) : IPaymentGateway
{
    public string Id => id;

    public async Task<PaymentResult> ProcessPaymentAsync(OrderRequest order)
    {
        logger.LogInformation("Processing payment for order '{OrderNumber}'.", order.OrderNumber);

        await Task.Delay(100);

        if (order.Amount < 0)
        {
            logger.LogError("Amount can't be less than zero.");
            return new PaymentResult(false, null, "Amount can't be less than zero.");
        }

        var result = new PaymentResult(true, Guid.NewGuid().ToString(), null);

        logger.LogInformation("Payment '{OrderNumber}' handles successfully.", order.OrderNumber)
            ;
        return result;
    }
}