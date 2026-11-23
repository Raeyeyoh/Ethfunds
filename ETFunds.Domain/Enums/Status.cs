namespace ETFunds.Domain.Enums;

public enum PaymentStatus
{
    Pending,
    Success,
    Failed,
    Cancelled //admin can do this if observe unusual activiy
}
