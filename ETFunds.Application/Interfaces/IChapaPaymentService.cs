using ETFunds.Application.Dtos;

namespace ETFunds.Application.Interfaces;

public interface IChapaPaymentService
{
    Task<ChapaPaymentResponse> InitializePaymentAsync(
        ChapaPaymentRequest request,
        CancellationToken cancellationToken);

    Task<ChapaPaymentVerificationResponse> VerifyPaymentAsync(
        string transactionReference,
        CancellationToken cancellationToken);
}