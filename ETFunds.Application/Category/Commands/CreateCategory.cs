using MediatR;

namespace ETFunds.Application.Category.Commands;

using Dtos;
using Interfaces;

public record CreateCategoryCommand(CreateCategoryRequest Request) : IRequest<CategoryResponse>;

public class CreateCategoryHandler(ICategoryService categoryService) : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryService.AddCategoryAsync(request.Request, cancellationToken);
        return new CategoryResponse { Name = category.Name, Id = category.Id };
    }
}