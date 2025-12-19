namespace Backend.Models;

public record PaymentReceipt(
    string OrderNumber,
    decimal Amount,
    long Timestamp,
    string PaymentConfirmation
);
