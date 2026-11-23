using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using MediatR;

namespace ETFunds.Application.Donation.Commands;

public record CreateDonationCommand(
    CreateDonationRequest Request,
    int UserId
) : IRequest<CreateDonationResponse>;

public class CreateDonationCommandHandler(
    IDonationService donationService)
    : IRequestHandler<CreateDonationCommand, CreateDonationResponse>
{
    public async Task<CreateDonationResponse> Handle(
        CreateDonationCommand command,
        CancellationToken cancellationToken)
    {
        return await donationService.CreateDonationAsync(
            command.Request,
            command.UserId,
            cancellationToken);
    }
}