namespace ETFunds.Application.Interfaces;

public interface IImageStorage
{
    Task<string> SaveAsync(
        Stream image,
        string fileName,
        CancellationToken ct);
}