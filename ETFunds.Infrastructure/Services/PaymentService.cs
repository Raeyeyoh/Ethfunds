using ETFunds.Application.Interfaces;
using ETFunds.Domain.Enums;
using ETFunds.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ETFunds.Infrastructure.Services;

public class PaymentService(
    ETFDbContext _context)
    : IPaymentService
{
    public async Task ProcessSuccessfulPaymentAsync(
        string transactionReference,
        CancellationToken cancellationToken)
    {
        // 1. Find the payment and its donation/campaign
        var payment = await _context.Payments
            .Include(p => p.Donation)
            .ThenInclude(d => d.Campaign)
            .FirstOrDefaultAsync(
                p => p.TransactionReference == transactionReference,
                cancellationToken);

        if (payment == null)
        {
            throw new KeyNotFoundException(
                "Payment not found.");
        }

        // 2. Prevent processing the same payment twice
        if (payment.Status == PaymentStatus.Success)
        {
            return;
        }

        // 3. Mark payment as paid
        payment.Status = PaymentStatus.Success;
        payment.PaidAt = DateTime.UtcNow;

        // 4. Get the campaign through the donation
        var campaign = payment.Donation.Campaign;

        // 5. Add the paid donation amount
        campaign.CurrentAmount += payment.Amount;

        // 6. Check if campaign reached its goal
        if (campaign.CurrentAmount >= campaign.GoalAmount)
        {
            campaign.Status = "Completed";
        }

        // 7. Save everything
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<PaymentStatusResponse?> GetPaymentStatusAsync(
    string transactionReference,
    CancellationToken cancellationToken)
    {
        var payment = await _context.Payments
            .AsNoTracking()
            .Include(p => p.Donation)
            .FirstOrDefaultAsync(
                p => p.TransactionReference == transactionReference,
                cancellationToken);

        if (payment == null)
        {
            return null;
        }

        return new PaymentStatusResponse
        {
            TransactionReference = payment.TransactionReference,
            Status = payment.Status.ToString(),
            Amount = payment.Amount,
            Currency = payment.Currency,
            CampaignId = payment.Donation.CampaignId
        };
    }
}