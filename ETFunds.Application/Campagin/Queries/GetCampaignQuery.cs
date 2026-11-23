using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using MediatR;

namespace ETFunds.Application.Campagin.Queries;

public record GetCampaignQuery(int Id) : IRequest<CampaignDetailsResponse>;


public class GetCampaignQueryHandler(ICampaignService campaignService) : IRequestHandler<GetCampaignQuery, CampaignDetailsResponse>
{
    public async Task<CampaignDetailsResponse> Handle(GetCampaignQuery request, CancellationToken cancellationToken)
    {
        var campaign = await campaignService.GetCampaignByIdAsync(request.Id, cancellationToken);
        if (campaign is null)
        {
            throw new Exception("Campaign not found");
        }
        return campaign;
    }
}