using Billing.Backend.Models;

namespace Billing.Backend.Gateways;

public interface IPaymentGateway
{
    string Id { get; }
    Task<PaymentResult> ProcessPaymentAsync(OrderRequest order);
}