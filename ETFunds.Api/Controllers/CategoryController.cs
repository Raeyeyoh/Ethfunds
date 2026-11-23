using ETFunds.Application.Category.Commands;
using ETFunds.Application.Category.Queries;
using ETFunds.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ETFunds.Api.Controllers;

[ApiController]
[Route("api/Categories")]

public class CategoryController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> AddCategory([FromBody] CreateCategoryRequest request, CancellationToken ct)
    {
        var command = new CreateCategoryCommand(request);
        var category = await mediator.Send(command, ct);
        return Ok(category);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCategories(CancellationToken ct)
    {
        var categories = await mediator.Send(new GetAllCategoriesQuery(), ct);
        return Ok(categories);
    }
}