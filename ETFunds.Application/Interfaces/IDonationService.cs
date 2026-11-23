using ETFunds.Application.Dtos;

namespace ETFunds.Application.Interfaces;

public interface IDonationService
{
    Task<CreateDonationResponse> CreateDonationAsync(CreateDonationRequest request, int userId, CancellationToken cancellationToken);
}