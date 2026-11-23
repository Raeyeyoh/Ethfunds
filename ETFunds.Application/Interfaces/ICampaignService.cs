namespace ETFunds.Application.Interfaces;

using Domain.Entities;
using Dtos;
public interface ICampaignService
{
    Task<Campaign> CreateCampaignAsync(CampaignRequest campaign, int userId, CancellationToken ct);
    Task<PagedResponse<CampaignResponse>> GetCampaignAsync(PagedRequest request, CancellationToken ct);
    Task<CampaignDetailsResponse?> GetCampaignByIdAsync(int campaignId, CancellationToken ct);
    Task<IEnumerable<CreatorCampaignResponse>> GetCampaignsByUserIdAsync(int userId, CancellationToken ct);

}