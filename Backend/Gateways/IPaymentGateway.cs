using Backend.Models;

namespace Backend.Gateways;

public interface IPaymentGateway
{
    string Id { get; }
    Task<PaymentResult> ProcessPaymentAsync(OrderRequest order);
}