namespace Billing.Backend.Gateways.Implementations;

public class BankPaymentGateway(ILogger<BankPaymentGateway> logger) : MockPaymentGateway("Bank", logger);