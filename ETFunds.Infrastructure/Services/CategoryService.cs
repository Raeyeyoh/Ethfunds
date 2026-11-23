using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using ETFunds.Domain.Entities;
using ETFunds.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ETFunds.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly ETFDbContext _context;
    public CategoryService(ETFDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryResponse> AddCategoryAsync(CreateCategoryRequest request, CancellationToken ct)
    {
        var category = new Category
        {
            Name = request.Name,
        };
        await _context.categories.AddAsync(category, ct);
        await _context.SaveChangesAsync(ct);

        return new CategoryResponse { Id = category.Id, Name = category.Name };

    }

    public async Task<List<CategoryResponse>> GetAllCategoriesAsync(CancellationToken ct)
    {
        return await _context.categories.AsNoTracking().Select(c => new CategoryResponse { Id = c.Id, Name = c.Name }).ToListAsync(ct);
    }
}