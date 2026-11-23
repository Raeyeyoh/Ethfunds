namespace ETFunds.Application.Interfaces;


using ETFunds.Application.Dtos;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllCategoriesAsync(CancellationToken ct);
    Task<CategoryResponse> AddCategoryAsync(CreateCategoryRequest request, CancellationToken ct);
}