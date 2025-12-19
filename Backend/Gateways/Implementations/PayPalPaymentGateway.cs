namespace Backend.Gateways.Implementations;

public class PayPalPaymentGateway(ILogger<PayPalPaymentGateway> logger) : MockPaymentGateway("PayPal", logger);