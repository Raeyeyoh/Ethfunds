namespace ETFunds.Application.Dtos;

public record PagedRequest
{
    private const int MaxPageSize = 10;
    private int _pageSize = 5;
    public int Page { get; init; } = 1;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value < 1 ? 5 : value > MaxPageSize ? MaxPageSize : value;
    }
    public string? Search { get; init; }
    public string OrderBy { get; init; } = "Title";
    public bool Descending { get; init; }
}