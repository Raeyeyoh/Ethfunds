using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using MediatR;
namespace ETFunds.Application.Category.Queries;

public record GetAllCategoriesQuery : IRequest<List<CategoryResponse>>;

public class GetAllCategoriesHandler(ICategoryService categoryService) : IRequestHandler<GetAllCategoriesQuery, List<CategoryResponse>>
{


    public async Task<List<CategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await categoryService.GetAllCategoriesAsync(cancellationToken);
    }
}