namespace Billing.Backend.Models;

public record OrderRequest(
    string OrderNumber,
    string UserId,
    decimal Amount,
    string PaymentGatewayId,
    string? Description = null
);