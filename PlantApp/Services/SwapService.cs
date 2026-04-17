using Microsoft.EntityFrameworkCore;
using PlantApp.Data;

namespace PlantApp.Services;

public class SwapService : ISwapService
{
    private readonly SupabaseSwapService _supabase;
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    private readonly AuthService _auth;

    public SwapService(
        SupabaseSwapService supabase,
        IDbContextFactory<AppDbContext> dbFactory,
        AuthService auth)
    {
        _supabase = supabase;
        _dbFactory = dbFactory;
        _auth = auth;
    }

    public async Task<int> GetOffersCountAsync()
    {
        var offers = await _supabase.GetOffersAsync();
        return offers.Count;
    }

    public async Task CreateOfferAsync(int ownerId, int plantId, string? desired)
    {
        var offer = new SwapOffer
        {
            OwnerId = ownerId,
            UserPlantId = plantId,
            DesiredPlantDescription = desired
        };

        await _supabase.CreateOfferAsync(offer);
    } 

    public async Task SendRequestAsync(int offerId, int fromUserId, int plantId)
    {
        using var db = await
            _dbFactory.CreateDbContextAsync();

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

    public async Task<List<SwapRequest>> GetIncomingRequestsAsync(int ownerId)
    {
        using var db = await _dbFactory.CreateDbContextAsync();

        return await db.SwapRequests
            .Include(x => x.SwapOffer)
            .Where(x => x.SwapOffer!.OwnerId == ownerId)
            .ToListAsync();
    }

    // Implement parameterless interface method and delegate to the overload with defaults
    public Task<List<SwapOffer>> GetAllOffersAsync()
    {
        return GetAllOffersAsync(false, 0, 20);
    }

    public async Task<List<SwapOffer>> GetAllOffersAsync(bool onlyPreview = false, int skip = 0, int take = 20)
    {
        var offers = await _supabase.GetOffersAsync();

        // пагинация НА СЕРВИСЕ
        offers = offers
            .Skip(skip)
            .Take(take)
            .ToList();

        using var db = await _dbFactory.CreateDbContextAsync();

        foreach (var offer in offers)
        {
            offer.Plant = await db.UserPlants
                .Include(x => x.Plant)
                .FirstOrDefaultAsync(x => x.Id == offer.UserPlantId);

            var owner = await db.Users
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == offer.OwnerId);

            offer.OwnerName = owner?.Login;
            offer.OwnerCity = owner?.Profile?.City;
            offer.OwnerProfile = owner?.Profile;
        }

        return offers;
    }

}