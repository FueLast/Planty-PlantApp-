using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

namespace PlantApp.Services;

public class SwapService : ISwapService
{
    private readonly SupabaseSwapService _supabase;
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public SwapService(
        SupabaseSwapService supabase,
        IDbContextFactory<AppDbContext> dbFactory)
    {
        _supabase = supabase;
        _dbFactory = dbFactory;
    }

    public async Task CreateOfferAsync(string userId, int plantId, string? desired)
    {
        var offer = new SwapOffer
        {
            OwnerId = userId,
            UserPlantId = plantId,
            DesiredPlantDescription = desired
        };

        await _supabase.CreateOfferAsync(offer);
    }

    public async Task<List<SwapOffer>> GetAllOffersAsync()
    {
        return await _supabase.GetOffersAsync();
    }

    public async Task SendRequestAsync(int offerId, int fromUserId, int plantId)
    {
        using var db = await _dbFactory.CreateDbContextAsync();

        var request = new SwapRequest
        {
            SwapOfferId = offerId,
            FromUserId = fromUserId,
            OfferedPlantId = plantId
        };

        db.SwapRequests.Add(request);
        await db.SaveChangesAsync();
    }

    public async Task UpdateRequestStatusAsync(int requestId, SwapStatus status)
    {
        using var db = await _dbFactory.CreateDbContextAsync();

        var request = await db.SwapRequests.FindAsync(requestId);

        if (request == null) return;

        request.Status = status;
        await db.SaveChangesAsync();
    }

    public async Task<List<SwapRequest>> GetIncomingRequestsAsync(string ownerId)
    {
        using var db = await _dbFactory.CreateDbContextAsync();

        return await db.SwapRequests
            .Include(x => x.SwapOffer)
            .Where(x => x.SwapOffer!.OwnerId == ownerId)
            .ToListAsync();
    }
}