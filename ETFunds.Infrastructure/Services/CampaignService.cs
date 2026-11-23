
using ETFunds.Application.Dtos;
using ETFunds.Application.Interfaces;
using ETFunds.Domain.Entities;
using ETFunds.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ETFunds.Infrastructure.Services;

public class CampaignService(ETFDbContext _context, IImageStorage _imageStorage) : ICampaignService
{


    public async Task<Campaign> CreateCampaignAsync(CampaignRequest campaign, int userId, CancellationToken ct)
    {
        string? imageUrl = null;

        if (campaign.Image != null)
        {
            imageUrl = await _imageStorage.SaveAsync(
                campaign.Image,
                campaign.ImageFileName ?? "image", ct);
        }
        var newCampaign = new Campaign
        {
            Title = campaign.Title,
            Description = campaign.Description,
            GoalAmount = campaign.GoalAmount,
            CurrentAmount = 0,
            ImageUrl = imageUrl ?? string.Empty,
            CategoryId = campaign.CategoryId,
            EndDate = campaign.EndDate,
            UserId = userId,
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            Country = campaign.Country,
            Region = campaign.Region,
            City = campaign.City
        };

        _context.Campaigns.Add(newCampaign);
        await _context.SaveChangesAsync(ct);

        return newCampaign;
    }

    public async Task<PagedResponse<CampaignResponse>> GetCampaignAsync(
    PagedRequest request,
    CancellationToken ct)
    {
        IQueryable<Campaign> query = _context.Campaigns
            .AsNoTracking()
            .Where(c => c.Status == "Active");

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(c =>
                EF.Functions.ILike(c.Title, $"%{request.Search}%") ||
                EF.Functions.ILike(c.category.Name, $"%{request.Search}%"));
        }

        var totalCount = await query.CountAsync(ct);

        IQueryable<Campaign> sortedQuery;

        switch (request.OrderBy)
        {
            case "Region":
                sortedQuery = request.Descending
                    ? query.OrderByDescending(c => c.Region)
                    : query.OrderBy(c => c.Region);
                break;

            case "City":
                sortedQuery = request.Descending
                    ? query.OrderByDescending(c => c.City)
                    : query.OrderBy(c => c.City);
                break;

            default:
                sortedQuery = request.Descending
                    ? query.OrderByDescending(c => c.Title)
                    : query.OrderBy(c => c.Title);
                break;
        }

        var items = await sortedQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CampaignResponse
            {
                Id = c.Id,
                ImageUrl = c.ImageUrl,
                Title = c.Title,
                CurrentAmount = c.CurrentAmount,
                GoalAmount = c.GoalAmount,
            })
            .ToListAsync(ct);

        return new PagedResponse<CampaignResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
    public Task<CampaignDetailsResponse?> GetCampaignByIdAsync(int campaignId, CancellationToken ct)
    {
        return _context.Campaigns
            .AsNoTracking()
            .Where(c => c.Id == campaignId)
            .Select(c => new CampaignDetailsResponse
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                GoalAmount = c.GoalAmount,
                CurrentAmount = c.CurrentAmount,
                ImageUrl = c.ImageUrl,
                CategoryId = c.CategoryId,
                EndDate = c.EndDate,
                Country = c.Country,
                Region = c.Region,
                City = c.City
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<CreatorCampaignResponse>> GetCampaignsByUserIdAsync(
    int userId,
    CancellationToken ct)
    {
        return await _context.Campaigns
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .Select(c => new CreatorCampaignResponse
            {
                Id = c.Id,
                Title = c.Title,
                GoalAmount = c.GoalAmount,
                CurrentAmount = c.CurrentAmount,
                Status = c.Status,
                EndDate = c.EndDate,
                ImageUrl = c.ImageUrl
            })
            .ToListAsync(ct);
    }
}