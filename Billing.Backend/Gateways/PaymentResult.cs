namespace Billing.Backend.Gateways;

public record PaymentResult(bool Success, string? ConfirmationId, string? ErrorMessage);