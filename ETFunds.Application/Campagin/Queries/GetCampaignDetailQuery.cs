using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using MediatR;

namespace ETFunds.Application.Campagin.Queries;

public record GetCampaignDetailQuery(int Id)
    : IRequest<CampaignDetailsResponse>;

public class GetCampaignDetailQueryHandler(
    ICampaignService campaignService)
    : IRequestHandler<GetCampaignDetailQuery, CampaignDetailsResponse>
{
    public async Task<CampaignDetailsResponse> Handle(
        GetCampaignDetailQuery request,
        CancellationToken cancellationToken)
    {
        var campaign = await campaignService.GetCampaignByIdAsync(
            request.Id,
            cancellationToken);

        if (campaign is null)
        {
            throw new Exception("Campaign not found");
        }

        return campaign;
    }
}