using System.Security.Claims;
using ETFunds.Application.Donation.Commands;
using ETFunds.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETFunds.Api.Controllers;

[ApiController]
[Route("api/Donations")]
public class DonationsController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<CreateDonationResponse>> CreateDonation(
        [FromBody] CreateDonationRequest request,
        CancellationToken cancellationToken)
    {

        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (userIdClaim is null ||
            !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("User ID was not found in the token.");
        }

        var command = new CreateDonationCommand(
            request,
            userId
        );

        var result = await mediator.Send(
            command,
            cancellationToken
        );

        return Ok(result);
    }
}