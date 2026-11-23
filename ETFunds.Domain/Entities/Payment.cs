using ETFunds.Domain.Enums;

namespace ETFunds.Domain.Entities;

public class Payment
{
    public int Id { get; set; }

    public int DonationId { get; set; }
    public Donation Donation { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "ETB";

    public string TransactionReference { get; set; } = string.Empty;

    public string? ChapaReference { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public string? PaymentMethod { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PaidAt { get; set; }
}