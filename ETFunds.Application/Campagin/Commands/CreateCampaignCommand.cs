using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using MediatR;

namespace ETFunds.Application.Campagin.Commands;

public record CreateCampaignCommand(CampaignRequest Request, int UserId) : IRequest<CampaignResponse>;
public class CreateCampaignCommandHandler(ICampaignService campaignService)
    : IRequestHandler<CreateCampaignCommand, CampaignResponse>
{


    public async Task<CampaignResponse> Handle(
        CreateCampaignCommand request,
        CancellationToken ct)
    {
        var campaign = await campaignService.CreateCampaignAsync(
            request.Request,
            request.UserId,
            ct
        );

        return new CampaignResponse
        {
            Id = campaign.Id,
            Title = campaign.Title,
            ImageUrl = campaign.ImageUrl,
            CurrentAmount = campaign.CurrentAmount,
            GoalAmount = campaign.GoalAmount
        };
    }
}