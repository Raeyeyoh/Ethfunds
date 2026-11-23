using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using MediatR;

namespace ETFunds.Application.Campagin.Queries;

public record GetCampaignsQuery(
    PagedRequest Request
) : IRequest<PagedResponse<CampaignResponse>>;


public class GetCampaignsQueryHandler(ICampaignService campaignService) : IRequestHandler<GetCampaignsQuery, PagedResponse<CampaignResponse>>
{
    public async Task<PagedResponse<CampaignResponse>> Handle(GetCampaignsQuery request, CancellationToken cancellationToken)
    {
        return await campaignService.GetCampaignAsync(request.Request, cancellationToken);
    }
}