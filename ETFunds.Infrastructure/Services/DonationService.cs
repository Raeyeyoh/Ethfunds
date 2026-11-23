
using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using ETFunds.Domain.Entities;
using ETFunds.Domain.Enums;
using ETFunds.Infrastructure.Persistence.Data;

using Microsoft.EntityFrameworkCore;

namespace ETFunds.Infrastructure.Services;

public class DonationService(ETFDbContext _context, IChapaPaymentService _chapaPaymentService) : IDonationService
{

    public async Task<CreateDonationResponse> CreateDonationAsync(
     CreateDonationRequest request,
     int userId,
     CancellationToken cancellationToken)
    {
        // 1. Validate campaign
        var campaign = await _context.Campaigns
            .FirstOrDefaultAsync(
                c => c.Id == request.CampaignId,
                cancellationToken);

        if (campaign == null)
        {
            throw new KeyNotFoundException("Campaign not found.");
        }

        // 2. Validate amount
        if (request.Amount <= 0)
        {
            throw new ArgumentException(
                "Donation amount must be greater than zero.");
        }

        // 3. Validate user
        var user = await _context.users
            .FirstOrDefaultAsync(
                u => u.Id == userId,
                cancellationToken);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        // 4. Create Donation
        var donation = new Donation
        {
            Amount = request.Amount,
            CampaignId = request.CampaignId,
            UserId = userId,
            IsAnonymous = request.IsAnonymous,
            DonatedAt = DateTime.UtcNow
        };

        // Generate our transaction reference
        var transactionReference = $"DON-{Guid.NewGuid():N}";

        // 5. Create Payment with Pending status
        var payment = new Payment
        {
            Donation = donation,
            Amount = request.Amount,
            Currency = "ETB",
            TransactionReference = transactionReference,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        // 6. Save Donation + Payment first
        _context.Donations.Add(donation);
        _context.Payments.Add(payment);

        await _context.SaveChangesAsync(cancellationToken);

        // At this point:
        // donation.Id exists
        // payment.Id exists
        // Payment.Status = Pending

        // 7. Initialize Chapa
        var chapaResult =
            await _chapaPaymentService.InitializePaymentAsync(
                new ChapaPaymentRequest
                {
                    Amount = request.Amount,
                    Currency = "ETB",
                    Email = user.Email,
                    FirstName = user.FullName,
                    TransactionReference = transactionReference
                },
                cancellationToken);

        // Chapa initialization failed
        if (!chapaResult.Success ||
            string.IsNullOrWhiteSpace(chapaResult.CheckoutUrl))
        {
            throw new InvalidOperationException(
                chapaResult.Message ??
                "Unable to initialize Chapa payment.");
        }

        // 8. Store Chapa reference
        payment.ChapaReference = chapaResult.ChapaReference;

        // 9. Save Chapa reference
        await _context.SaveChangesAsync(cancellationToken);

        // 10. Return checkout URL
        return new CreateDonationResponse
        {
            DonationId = donation.Id,
            PaymentId = payment.Id,
            Amount = donation.Amount,
            Status = payment.Status.ToString(),
            CheckoutUrl = chapaResult.CheckoutUrl
        };
    }


}