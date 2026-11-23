namespace ETFunds.Application.Interfaces;

public interface IPaymentService
{
    Task ProcessSuccessfulPaymentAsync(
        string transactionReference,
        CancellationToken cancellationToken);
    Task<PaymentStatusResponse?> GetPaymentStatusAsync(
string transactionReference,
CancellationToken cancellationToken);
}