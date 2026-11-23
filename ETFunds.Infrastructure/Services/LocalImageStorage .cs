using ETFunds.Application.Interfaces;

namespace ETFunds.Infrastructure.Services;

public class LocalImageStorage : IImageStorage
{
    private readonly string _uploadPath;

    public LocalImageStorage(string uploadPath)
    {
        _uploadPath = uploadPath;
    }

    public async Task<string> SaveAsync(
        Stream image,
        string fileName,
        CancellationToken ct)
    {
        Directory.CreateDirectory(_uploadPath);

        var extension = Path.GetExtension(fileName);
        var storedFileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(_uploadPath, storedFileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create);

        await image.CopyToAsync(fileStream, ct);

        return $"/uploads/campaigns/{storedFileName}";
    }
}