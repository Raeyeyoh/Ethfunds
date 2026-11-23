using System.Security.Claims;
using ETFunds.Api.Dtos;
using ETFunds.Application.Campagin.Commands;
using ETFunds.Application.Campagin.Queries;
using ETFunds.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETFunds.Api.Controllers;

[ApiController]
[Route("api/Campaigns")]
public class CampaignController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCampaignAsync(
    [FromForm] CreateCampaignRequest request,
    CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        Stream? imageStream = null;

        if (request.Image != null)
        {
            imageStream = request.Image.OpenReadStream();
        }

        var campaignRequest = new CampaignRequest
        {
            Title = request.Title,
            Description = request.Description,
            GoalAmount = request.GoalAmount,
            Image = imageStream,
            ImageFileName = request.Image?.FileName,
            CategoryId = request.CategoryId,
            EndDate = DateTime.SpecifyKind(
                request.EndDate,
                DateTimeKind.Utc
            ),
            Country = request.Country,
            Region = request.Region,
            City = request.City
        };

        var command = new CreateCampaignCommand(
            campaignRequest,
            userId
        );

        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetCampaignsAsync(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetCampaignsQuery(request);

        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCampaignByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var query = new GetCampaignDetailQuery(id);

        var result = await mediator.Send(query, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

}